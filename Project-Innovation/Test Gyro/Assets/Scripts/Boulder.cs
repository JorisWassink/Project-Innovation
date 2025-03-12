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
    private float normalDrag; // Store default drag
    [SerializeField] private Transform cameraTransform; // Assign in Inspector
    private Vector3 targetForce;
    private Rigidbody rb;

    // UDP variables
    private UdpClient udpClient;
    private Thread receiveThread;
    private Vector3 receivedGyro;

    public int port = 6060; // Set this to your UDP port

    // Gyro scaling factor for more control over force application
    public float gyroScaling = 0.001f; // Lower scale for better control over force values
    private float deadZone = 0.1f; // Dead zone to prevent jitter

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        normalDrag = rb.linearDamping; // Save initial drag value

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

        // Normalize gyro values
        float tiltX = Mathf.Repeat(-receivedGyro.x, 360f) / 360f * 2f - 1f;
        float tiltY = Mathf.Repeat(receivedGyro.y, 360f) / 360f * 2f - 1f;

        // Apply dead zone to prevent small unwanted movements
        if (Mathf.Abs(tiltX) < deadZone) tiltX = 0;
        if (Mathf.Abs(tiltY) < deadZone) tiltY = 0;

        // Get a stable forward direction for movement (ignoring vertical tilt)
        Vector3 flatForward = new Vector3(cameraTransform.forward.x, 0, cameraTransform.forward.z).normalized;
        Vector3 flatRight = new Vector3(cameraTransform.right.x, 0, cameraTransform.right.z).normalized;

        // Convert gyro input to world-space movement
        Vector3 desiredForce = (flatForward * tiltX + flatRight * tiltY) * boulderSpeed;

        // Smooth force transition to avoid jerky movements
        targetForce = Vector3.Lerp(targetForce, desiredForce, tiltSmoothing * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        if (rb == null) return;

        // Apply the force to the ball if it's not zero
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ice"))
        {
            rb.linearDamping = 0.1f; // Reduce drag for sliding effect
        }
        else if (other.gameObject.CompareTag("Mud"))
        {
            rb.linearDamping = 3f; // Increase drag for mud
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Ice") || other.gameObject.CompareTag("Mud"))
        {
            rb.linearDamping = normalDrag; // Reset drag when leaving
        }
    }

    private void OnApplicationQuit()
    {
        receiveThread?.Abort();
        udpClient?.Close();
    }
}
