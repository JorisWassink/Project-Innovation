using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Unity.VisualScripting;

public class TiltScript : NetworkBehaviour
{
    public static readonly List<TiltScript> playersList = new List<TiltScript>();
    public event System.Action<byte> OnPlayerNumberChanged;

    [SyncVar] private Vector3 receivedGyro; // Sync gyro data across clients
    private Vector3 accumulatedRotation;

    public float rotationSpeed = 500f;
    public float tiltSmoothing = 5f; // Smooth tilt transitions
    public Transform cameraTransform; // Assign in Inspector
    public Transform ballTransform;   // Assign in Inspector
    public float moveSpeed = 5f; // Movement speed when spacebar is pressed

    private Quaternion initialGyroRotation;

    [Header("SyncVars")]
    [SyncVar(hook = nameof(PlayerNumberChanged))]
    public byte playerNumber = 0;

    void PlayerNumberChanged(byte _, byte newPlayerNumber)
    {
        OnPlayerNumberChanged?.Invoke(newPlayerNumber);
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        playersList.Add(this);
    }

    [ServerCallback]
    internal static void ResetPlayerNumbers()
    {
        byte playerNumber = 0;
        foreach (TiltScript player in playersList)
            player.playerNumber = playerNumber++;
    }

    public override void OnStopServer()
    {
        playersList.Remove(this);
    }

    private void Start()
    {
        Debug.Log($"Player object started. isLocalPlayer: {isLocalPlayer}");

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
        Debug.Log($"Is Local Player{isLocalPlayer}");
        if (!isLocalPlayer) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Space Pressed");
            var t = this.transform.position;
            t.y += 100f;
            this.transform.position = t;
        }

        // Get normalized gyro rotation
        Quaternion rawGyro = GyroToUnity(Input.gyro.attitude);
        Quaternion relativeRotation = Quaternion.Inverse(initialGyroRotation) * rawGyro;
        Vector3 gyroEuler = relativeRotation.eulerAngles;

        // Log the gyro data to ensure it's working
        Debug.Log("Gyroscope Euler: " + gyroEuler);

        // Send gyro data to server
        CmdSendGyroData(gyroEuler);

        // Check for spacebar press and move the ball
        
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

        // Scale and smooth the tilt data
        Vector3 tiltDirection = (-receivedGyro.x * cameraRight) + (-receivedGyro.y * cameraForward);

        // Debug logs
        Debug.Log("Received Gyro: " + receivedGyro);  // Log gyro data
        Debug.Log("Tilt Direction: " + tiltDirection);  // Log tilt direction

        // Smooth the accumulated rotation for a more natural transition
        accumulatedRotation = Vector3.Lerp(accumulatedRotation, accumulatedRotation + tiltDirection * rotationSpeed * Time.fixedDeltaTime, tiltSmoothing * Time.fixedDeltaTime);

        // Apply rotation smoothly **around the ball’s position**
        transform.RotateAround(ballTransform.position, cameraRight, accumulatedRotation.x);
        transform.RotateAround(ballTransform.position, cameraForward, accumulatedRotation.y);
    }
}
