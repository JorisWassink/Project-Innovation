using System.Collections.Generic;
using UnityEngine;
using Mirror;

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

    private Quaternion initialGyroRotation;
    
    [Header("SyncVars")]


    [SyncVar(hook = nameof(PlayerNumberChanged))]
    public byte playerNumber = 0;

    
    void PlayerNumberChanged(byte _, byte newPlayerNumber)
    {
        OnPlayerNumberChanged?.Invoke(newPlayerNumber);
    }
    /// <summary>
    /// This is invoked for NetworkBehaviour objects when they become active on the server.
    /// <para>This could be triggered by NetworkServer.Listen() for objects in the scene, or by NetworkServer.Spawn() for objects that are dynamically created.</para>
    /// <para>This will be called for objects on a "host" as well as for object on a dedicated server.</para>
    /// </summary>
    public override void OnStartServer()
    {
        base.OnStartServer();

        // Add this to the static Players List
        playersList.Add(this);

    }
    
    // This is called from BasicNetManager OnServerAddPlayer and OnServerDisconnect
    // Player numbers are reset whenever a player joins / leaves
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
