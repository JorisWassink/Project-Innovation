using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class GyroReceiver : MonoBehaviour {
    public int port = 6060;
    private UdpClient udpClient;
    private Thread receiveThread;
    private Vector3 receivedGyro;
    private Vector3 accumulatedRotation;
    public float rotationSpeed = 500f;
    public Transform cameraTransform; // Assign the Camera in Inspector
    public Transform ballTransform;

    void Start() {
        udpClient = new UdpClient(port);
        receiveThread = new Thread(ReceiveData);
        receiveThread.IsBackground = true;
        receiveThread.Start();
    }

    void ReceiveData() {
        while (true) {
            IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, port);
            byte[] data = udpClient.Receive(ref remoteEndPoint);
            string message = Encoding.UTF8.GetString(data);
            string[] values = message.Split(',');

            if (values.Length == 3) {
                receivedGyro = new Vector3(float.Parse(values[0]), float.Parse(values[1]), float.Parse(values[2]));
            }
        }
    }

    void FixedUpdate() {
    if (cameraTransform == null || ballTransform == null) return;

    // Get the right and forward directions relative to the camera
    Vector3 cameraRight = cameraTransform.right;
    Vector3 cameraForward = Vector3.Cross(cameraRight, Vector3.up); // Flattened forward

    // Convert received gyro data to world-space tilting
    Vector3 tiltDirection = (-receivedGyro.x * cameraRight) + (-receivedGyro.y * cameraForward);

    // Accumulate rotation
    accumulatedRotation += tiltDirection * rotationSpeed * Time.fixedDeltaTime;

    // Calculate the new rotation
    Quaternion targetRotation = Quaternion.Euler(accumulatedRotation.x, 0, accumulatedRotation.z);

    // Apply rotation around the ball's position
    transform.RotateAround(ballTransform.position, cameraRight, -receivedGyro.x * rotationSpeed * Time.fixedDeltaTime);
    transform.RotateAround(ballTransform.position, cameraForward, -receivedGyro.y * rotationSpeed * Time.fixedDeltaTime);
}


    void OnApplicationQuit() {
        receiveThread.Abort();
        udpClient.Close();
    }
}
