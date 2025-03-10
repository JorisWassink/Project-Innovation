using System;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class Boulder : MonoBehaviour
{
    [SerializeField] private float boosterStrength = 50f;
    [SerializeField] private float boulderSpeed = 5f;
    [SerializeField] private float tiltSmoothing = 5f; // Smooth tilt transitions
    [SerializeField] private Transform cameraTransform; // Assign in Inspector
    private Vector3 targetForce;
    private Rigidbody rb;

    // UDP variables
    private UdpClient udpClient;
    private Thread receiveThread;
    private Vector3 receivedGyro;

    public int port = 6060; // Set this to your UDP port

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Start UDP listener for gyro data
        udpClient = new UdpClient(port);
        receiveThread = new Thread(ReceiveData);
        receiveThread.IsBackground = true;
        receiveThread.Start();
    }

    private void ReceiveData()
    {
        while (true)
        {
            try
            {
                IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, port);
                byte[] data = udpClient.Receive(ref remoteEndPoint);
                string message = Encoding.UTF8.GetString(data);
                string[] values = message.Split(',');

                if (values.Length == 3)
                {
                    receivedGyro = new Vector3(float.Parse(values[0]), float.Parse(values[1]), float.Parse(values[2]));
                }
            }
            catch (Exception e)
            {
                Debug.LogError("UDP Error: " + e.Message);
            }
        }
    }

    private void Update()
    {
        if (cameraTransform == null) return;

        // Gyro tilt data (assuming phone is flat-facing up, adjust axis if needed)
        float tiltX = Mathf.Repeat(-receivedGyro.x, 360f) / 360f * 2f - 1f; 
        float tiltY = Mathf.Repeat(receivedGyro.y, 360f) / 360f * 2f - 1f; 

        // Clamp tilt values to a range that makes sense for ball control
        tiltX = Mathf.Clamp(tiltX, -1f, 1f);
        tiltY = Mathf.Clamp(tiltY, -1f, 1f);

        // Get camera-aligned right and forward directions
        Vector3 cameraRight = cameraTransform.right;
        Vector3 cameraForward = cameraTransform.forward;

        cameraForward.y = 0; 
        cameraForward.Normalize();

        cameraRight.y = 0;
        cameraRight.Normalize();

        // Adjust movement force (make it more controllable)
        Vector3 desiredForce = (cameraForward * tiltX + cameraRight * tiltY) * boulderSpeed;
        targetForce = Vector3.Lerp(targetForce, desiredForce, tiltSmoothing * Time.deltaTime);

    }

    private void FixedUpdate()
    {
        if (rb == null) return;

        if (targetForce != Vector3.zero)
        {
            rb.AddForce(targetForce, ForceMode.Force);
            Debug.Log($"Gyro Force Applied: {targetForce}");
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("SpeedBooster"))
        {
            SpeedBoost(other.transform.forward);
        }
    }

    private void SpeedBoost(Vector3 direction)
    {
        rb.AddForce(direction * boosterStrength, ForceMode.Impulse);
    }

    private void OnApplicationQuit()
    {
        receiveThread?.Abort();
        udpClient?.Close();
    }
}
