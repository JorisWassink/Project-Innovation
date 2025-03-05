using UnityEngine;

public class KeyboardControls : MonoBehaviour
{
    public float rotationSpeed = 500f;
    public float tiltSmoothing = 5f; // Smooth tilt transitions
    public Transform cameraTransform; // Assign in Inspector
    public Transform ballTransform;   // Assign in Inspector

    private Vector3 targetRotation;

    private void Update()
    {
        // Get input from arrow keys or WASD
        float tiltX = Input.GetAxis("Vertical");   // W/S or Up/Down
        float tiltY = Input.GetAxis("Horizontal"); // A/D or Left/Right

        // Get the camera-aligned right and forward directions
        Vector3 cameraRight = cameraTransform.right;
        Vector3 cameraForward = cameraTransform.forward;
        cameraForward.y = 0; // Flatten to horizontal plane
        cameraForward.Normalize();

        // Convert input to world-space tilting
        targetRotation = (-tiltX * cameraRight) + (-tiltY * cameraForward);
        
    }

    private void FixedUpdate()
    {
        if (cameraTransform == null || ballTransform == null) return;

        Vector3 cameraRight = cameraTransform.right;
        Vector3 cameraForward = cameraTransform.forward;
        cameraForward.y = 0; 
        cameraForward.Normalize();

        // Smooth transition of tilt direction
        Vector3 smoothedRotation = Vector3.Lerp(Vector3.zero, targetRotation * rotationSpeed, tiltSmoothing * Time.fixedDeltaTime);

        // Apply rotation
        transform.RotateAround(ballTransform.position, cameraRight, -smoothedRotation.x * Time.fixedDeltaTime);
        transform.RotateAround(ballTransform.position, cameraForward, smoothedRotation.z * Time.fixedDeltaTime);
    }
}