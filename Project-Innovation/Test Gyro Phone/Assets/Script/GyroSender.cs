using UnityEngine;
using UnityEngine.UI;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine.InputSystem;
using TMPro;

public class GyroSender : MonoBehaviour
{
    private UdpClient udpClient;
    public string pcIP = "145.76.216.210"; // Replace with your PC's local IP
    public int port = 6060; // Match this with the PC's UDP receiver port

    private Quaternion initialRotation;
    private bool gyroEnabled = false;
    public TMP_Text GyroText; // Reference to UI Text component

    void Start()
    {
        udpClient = new UdpClient();
        Input.gyro.enabled = true;
        initialRotation = Input.gyro.attitude;
        gyroEnabled = true;
    }

    private void OnEnable()
    {
        InputSystem.EnableDevice(UnityEngine.InputSystem.Gyroscope.current);
    }

    void Update()
    {
        // Get gyro rotation relative to the initial state
        Quaternion rawGyro = GyroToUnity(Input.gyro.attitude);
        Quaternion relativeRotation = Quaternion.Inverse(initialRotation) * rawGyro;
        Vector3 gyroEuler = relativeRotation.eulerAngles;

        if (GyroText != null)
        {
            GyroText.text = $"Gyro X: {gyroEuler.x:F2} Gyro Y: {gyroEuler.y:F2} Gyro Z: {gyroEuler.z:F2}";
        }
        else if(GyroText == null){
            GyroText.text = "No Gyro";
        }

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
