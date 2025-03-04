using UnityEngine;
using Mirror;

public class TiltScript : NetworkBehaviour
{
    [SyncVar] private Vector3 receivedGyro; // Sync gyro data across clients
    private Vector3 accumulatedRotation;

    public float rotationSpeed = 500f;
    public float tiltSmoothing = 5f; // Smooth tilt transitions
    public Transform cameraTransform; // Assign in Inspector
    public Transform ballTransform;   // Assign in Inspector

    private Quaternion initialGyroRotation;

    private void Start()
    {
        if (!isLocalPlayer) return;

        if (SystemInfo.supportsGyroscope)
        {
            Input.gyro.enabled = true;
            initialGyroRotation = GyroToUnity(Input.gyro.attitude);
        }
        else
        {
            Debug.LogError("Gyroscope not supported on this device!");
        }
    }

    private void Update()
    {
        if (!isLocalPlayer) return;

        // Get normalized gyro rotation
        Quaternion rawGyro = GyroToUnity(Input.gyro.attitude);
        Quaternion relativeRotation = Quaternion.Inverse(initialGyroRotation) * rawGyro;
        Vector3 gyroEuler = relativeRotation.eulerAngles;

        // Send gyro data to server
        CmdSendGyroData(gyroEuler);
    }

    private Quaternion GyroToUnity(Quaternion q)
    {
        return new Quaternion(q.x, q.y, -q.z, -q.w);
    }

    [Command]
    private void CmdSendGyroData(Vector3 gyroData)
    {
        receivedGyro = gyroData; // Update server
        RpcUpdateGyroData(gyroData); // Sync with all clients
    }

    [ClientRpc]
    private void RpcUpdateGyroData(Vector3 gyroData)
    {
        receivedGyro = gyroData;
    }

    private void FixedUpdate()
    {
        if (cameraTransform == null || ballTransform == null) return;

        // Get the right and forward directions relative to the camera
        Vector3 cameraRight = cameraTransform.right;
        Vector3 cameraForward = Vector3.Cross(cameraRight, Vector3.up); // Flattened forward

        // Convert received gyro data to world-space tilting
        Vector3 tiltDirection = (-receivedGyro.x * cameraRight) + (-receivedGyro.y * cameraForward);

        // Accumulate rotation smoothly
        accumulatedRotation = Vector3.Lerp(accumulatedRotation, accumulatedRotation + tiltDirection * rotationSpeed * Time.fixedDeltaTime, tiltSmoothing * Time.fixedDeltaTime);

        // Rotate maze **around the ball’s position**
        transform.RotateAround(ballTransform.position, cameraRight, -receivedGyro.x * rotationSpeed * Time.fixedDeltaTime);
        transform.RotateAround(ballTransform.position, cameraForward, -receivedGyro.y * rotationSpeed * Time.fixedDeltaTime);
    }
}
