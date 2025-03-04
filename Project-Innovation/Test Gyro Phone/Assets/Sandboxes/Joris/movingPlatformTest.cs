using UnityEngine;
using Mirror;

public class MovingPlatformTest : NetworkBehaviour
{
    public Transform cameraTransform; // Assign in Inspector
    public Transform ballTransform;   // Assign in Inspector
    public float rotationSpeed = 500f;
    public float cameraFollowSpeed = 5f;
    public float tiltSmoothing = 5f;  // Smooths out the tilting

    private Vector3 receivedGyro;
    private Vector3 accumulatedRotation;
    private Quaternion initialGyroRotation;

    private void Start()
    {
        if (!isLocalPlayer) return;

        if (SystemInfo.supportsGyroscope)
        {
            Input.gyro.enabled = true;
            initialGyroRotation = GyroToUnity(Input.gyro.attitude); // Store initial calibration
        }
        else
        {
            Debug.LogError("Gyroscope not supported on this device!");
        }
    }

    private void Update()
    {
        if (!isLocalPlayer) return;

        Quaternion rawGyro = GyroToUnity(Input.gyro.attitude);
        Quaternion relativeRotation = Quaternion.Inverse(initialGyroRotation) * rawGyro; // Normalize to start position

        receivedGyro = relativeRotation.eulerAngles;

        // Send rotation update to server
        CmdUpdateRotation(relativeRotation);
    }

    private Quaternion GyroToUnity(Quaternion q)
    {
        // Convert phone gyro orientation to Unity space
        return new Quaternion(q.x, q.y, -q.z, -q.w);
    }

    [Command]
    private void CmdUpdateRotation(Quaternion newRotation)
    {
        transform.rotation = newRotation;
        RpcUpdateRotation(newRotation);
    }

    [ClientRpc]
    private void RpcUpdateRotation(Quaternion newRotation)
    {
        if (!isLocalPlayer)
        {
            transform.rotation = newRotation;
        }
    }

    private void FixedUpdate()
    {
        if (cameraTransform == null || ballTransform == null) return;

        // === 1. Rotate the Maze around the Ball's Position ===
        Vector3 cameraRight = cameraTransform.right;
        Vector3 cameraForward = Vector3.Cross(cameraRight, Vector3.up);

        // Use gyro data to adjust rotation around the ball
        Vector3 tiltDirection = (-receivedGyro.x * cameraRight) + (-receivedGyro.y * cameraForward);

        // Apply accumulated rotation with smoothing
        accumulatedRotation = Vector3.Lerp(accumulatedRotation, accumulatedRotation + (tiltDirection * rotationSpeed * Time.fixedDeltaTime), tiltSmoothing * Time.fixedDeltaTime);

        // Rotate around the ball's position
        transform.RotateAround(ballTransform.position, cameraRight, -receivedGyro.x * rotationSpeed * Time.fixedDeltaTime);
        transform.RotateAround(ballTransform.position, cameraForward, -receivedGyro.y * rotationSpeed * Time.fixedDeltaTime);

        // === 2. Smooth Camera Follow ===
        Vector3 targetPosition = ballTransform.position + new Vector3(0, 5, -5);
        cameraTransform.position = Vector3.Lerp(cameraTransform.position, targetPosition, cameraFollowSpeed * Time.fixedDeltaTime);
        cameraTransform.LookAt(ballTransform.position);
    }
}
