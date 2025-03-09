using System;
using Unity.Mathematics;
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
        var ballRb = ballTransform.GetComponent<Rigidbody>();
        var sphereForce = -ballRb.linearVelocity.normalized;
        
        if (ballRb.linearVelocity.magnitude < 0.1f) {
            sphereForce = Vector3.back; 
        }
        var finalOffset = offset.magnitude * sphereForce;
        finalOffset.y = offset.y;

        // Desired camera position
        Vector3 desiredPosition = ballTransform.position + finalOffset;

        float wallOffset = 0.3f; // Hoe ver de camera van muren blijft

        Vector3 direction = (desiredPosition - ballTransform.position).normalized;
        float distance = offset.magnitude;

        if (Physics.Raycast(ballTransform.position, direction, out RaycastHit hit, distance)) {
            desiredPosition = ballTransform.position + direction * (hit.distance - wallOffset);
        }
        
        // Smoothly move the camera to follow the ball
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        // Keep the camera at the original fixed rotation   
        transform.LookAt(ballTransform.position);
        
    }


}
