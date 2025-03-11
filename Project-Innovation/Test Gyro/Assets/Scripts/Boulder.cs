using UnityEngine;

public class Boulder : MonoBehaviour
{
    [SerializeField] private float boosterStrength = 50f;
    [SerializeField] private float boulderSpeed = 50f;
    [SerializeField] private float tiltSmoothing = 5f; // Smooth tilt transitions
    [SerializeField] private Transform cameraTransform; // Assign in Inspector
    private Vector3 targetForce;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Enable gyroscope
        if (SystemInfo.supportsGyroscope)
        {
            Input.gyro.enabled = true;
            Debug.Log("Gyroscope enabled.");
        }
        else
        {
            Debug.LogError("Gyroscope not supported!");
        }
    }

    void Update()
    {
        if (cameraTransform == null) return;

        // Get phone gyro rotation
        Quaternion gyroRotation = Input.gyro.attitude;
        
        // Debug log to see raw gyro rotation data
        Debug.Log($"Gyro Rotation: {gyroRotation}");

        // Convert gyro input to angles (Euler angles)
        Vector3 gyroEuler = gyroRotation.eulerAngles;
        
        // Debug log to see the converted angles
        Debug.Log($"Gyro Euler Angles: {gyroEuler}");

        // Normalize gyro values for tilt (mapping them to a range from -1 to 1)
        float tiltX = Mathf.Repeat(-gyroEuler.x, 360f) / 360f * 2f - 1f;
        float tiltY = Mathf.Repeat(-gyroEuler.y, 360f) / 360f * 2f - 1f;

        // Debug logs for tilt values
        Debug.Log($"Tilt X: {tiltX}, Tilt Y: {tiltY}");

        // Manually define the camera's right and forward directions for a top-down view
        Vector3 cameraRight = cameraTransform.right; // X-axis direction
        Vector3 cameraForward = cameraTransform.forward; // Z-axis direction

        cameraForward.y = 0; 
        cameraForward.Normalize();

        cameraRight.y = 0;
        cameraRight.Normalize();

        // Debug log for camera directions
        Debug.Log($"Camera Forward: {cameraForward}, Camera Right: {cameraRight}");

        // Convert gyro input to world-space movement
        Vector3 desiredForce = (cameraForward * tiltY + cameraRight * tiltX) * boulderSpeed;
        targetForce = Vector3.Lerp(targetForce, desiredForce, tiltSmoothing * Time.deltaTime);

        // Debug log for target force applied
        Debug.Log($"Desired Force: {desiredForce}, Target Force: {targetForce}");
    }

    private void FixedUpdate()
    {
        if (rb == null) return;

        if (targetForce != Vector3.zero)
        {
            rb.AddForce(targetForce, ForceMode.Force);
            // Debug log for force applied to the rigidbody
            Debug.Log($"Force Applied to Rigidbody: {targetForce}");
        }
    }

    private void OnApplicationQuit()
    {
        Input.gyro.enabled = false;
        Debug.Log("Gyroscope disabled.");
    }
}
