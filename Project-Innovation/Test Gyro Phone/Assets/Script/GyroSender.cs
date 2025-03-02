using UnityEngine;
using System.Net.Sockets;
using System.Text;

public class GyroSender : MonoBehaviour {
    private UdpClient udpClient;
    public string ipAddress = "145.76.216.210"; // Replace with your PC's local IP
    public int port = 6060;

    void Start() {
        Input.gyro.enabled = true;
        udpClient = new UdpClient();
    }

    void Update() {
    Vector3 gyroData = Input.gyro.rotationRateUnbiased;
    string message = $"{gyroData.x},{gyroData.y},{gyroData.z}";
    Debug.Log("Gyro Data: " + message); // Add this line
    byte[] data = Encoding.UTF8.GetBytes(message);
    udpClient.Send(data, data.Length, ipAddress, port);
}

}
