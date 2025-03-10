using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class GyroSender : MonoBehaviour
{
    private UdpClient udpClient;
    public string pcIP = "192.168.1.100"; // Replace with your PC's local IP
    public int port = 6060; // Match this with the PC's UDP receiver port

    private Quaternion initialRotation;
    private bool gyroEnabled = false;

    void Start()
    {
        udpClient = new UdpClient();
        if (SystemInfo.supportsGyroscope)
        {
            Input.gyro.enabled = true;
            initialRotation = Input.gyro.attitude;
            gyroEnabled = true;
        }
        else
        {
            Debug.LogError("Gyroscope not supported on this device!");
        }
    }

    void Update()
    {
        if (!gyroEnabled) return;

        // Get gyro rotation relative to the initial state
        Quaternion rawGyro = GyroToUnity(Input.gyro.attitude);
        Quaternion relativeRotation = Quaternion.Inverse(initialRotation) * rawGyro;
        Vector3 gyroEuler = relativeRotation.eulerAngles;

        // Send gyro data as a string
        string message = $"{gyroEuler.x},{gyroEuler.y},{gyroEuler.z}";
        byte[] data = Encoding.UTF8.GetBytes(message);
        udpClient.Send(data, data.Length, pcIP, port);

        Debug.Log("Sent Gyro: " + message);
    }

    private Quaternion GyroToUnity(Quaternion q)
    {
        return new Quaternion(q.x, q.y, -q.z, -q.w);
    }

    private void OnApplicationQuit()
    {
        udpClient?.Close();
    }
}
