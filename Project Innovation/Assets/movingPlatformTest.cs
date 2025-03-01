using UnityEngine;
using Mirror;

public class MovingPlatformTest : NetworkBehaviour
{
    private void Start()
    {
        // Only enable gyroscope on the local player (phone)
        if (!isLocalPlayer) return;

        if (SystemInfo.supportsGyroscope)
        {
            Input.gyro.enabled = true;  // Enable the gyroscope if it's supported
        }
        else
        {
            Debug.LogError("Gyroscoop wordt niet ondersteund op dit apparaat!");
        }
    }

    private void Update()
    {
        
        // If this isn't the local player, or gyroscope isn't enabled, do nothing
        if (!isLocalPlayer) return;

        // Use the gyroscope data to adjust the rotation
        CmdUpdateRotation(GyroToUnity(Input.gyro.attitude));

        if (Input.GetKeyDown(KeyCode.Space))
        {
            transform.position += Vector3.up;
        }
    }

    private Quaternion GyroToUnity(Quaternion q)
    {
        // Convert the gyroscope values to Unity's coordinate system
        return new Quaternion(q.x, q.y, -q.z, -q.w);
    }

    // Command: Called by the client (phone) to tell the server to update the rotation
    [Command]
    private void CmdUpdateRotation(Quaternion newRotation)
    {
        // Update the rotation on the server
        transform.rotation = newRotation;

        // Tell all clients to update the rotation (including the phone)
        RpcUpdateRotation(newRotation);
    }

    // ClientRpc: Called by the server to update all clients (including the phone)
    [ClientRpc]
    private void RpcUpdateRotation(Quaternion newRotation)
    {
        // Update the rotation on all clients except the local player (who already updated it)
        if (!isLocalPlayer)
        {
            transform.rotation = newRotation;
        }
    }
}