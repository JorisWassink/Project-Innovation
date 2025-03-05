using UnityEngine;

public class CameraFollow : MonoBehaviour {
    public Transform ballTransform; // Assign the ball in Inspector
    public Vector3 offset = new Vector3(0, 5, -7); // Adjust for better view
    public float smoothSpeed = 5f;

    private Quaternion fixedRotation; // Store the fixed rotation

    void Start() {
        // Store the initial rotation to keep the camera at a fixed angle
        fixedRotation = transform.rotation;
    }

    void LateUpdate() {
        if (ballTransform == null) return;

        // Desired camera position
        Vector3 targetPosition = ballTransform.position + offset;

        // Smoothly move the camera to follow the ball
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);

        // Keep the camera at the original fixed rotation
        transform.rotation = fixedRotation;
    }
}
