using System;
using UnityEngine;

public class Boulder : MonoBehaviour
{
    [SerializeField] private float boosterStrength = 50f;
    [SerializeField] private float boulderSpeed = 5f;
    [SerializeField] private float rotationSpeed = 500f;
    [SerializeField] private float tiltSmoothing = 5f; // Smooth tilt transitions
    [SerializeField] private Transform cameraTransform; // Assign in Inspector
    private Vector3 targetForce;
    private Rigidbody rb;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        if (cameraTransform == null) return;

        // Get input from arrow keys or WASD
        float tiltX = Input.GetAxis("Vertical");   // W/S or Up/Down
        float tiltY = Input.GetAxis("Horizontal"); // A/D or Left/Right

        // Get the camera-aligned right and forward directions
        Vector3 cameraRight = cameraTransform.right;
        Vector3 cameraForward = cameraTransform.forward;
        
        cameraForward.y = 0; 
        cameraForward.Normalize();
        
        cameraRight.y = 0;
        cameraRight.Normalize();

        // Convert input to world-space movement
        targetForce = (cameraForward * tiltX + cameraRight * tiltY) * boulderSpeed;

        // Reduce drift when no input is given
        if (tiltX == 0 && tiltY == 0)
        {
            targetForce = Vector3.zero;
        }
    }

    private void FixedUpdate()
    {
        if (rb == null) return;

        if (targetForce != Vector3.zero)
        {
            rb.AddForce(targetForce, ForceMode.Acceleration);
        }
    }
    

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("SpeedBooster"))
        {
            SpeedBoost(other.transform.forward);
        }
    }
    
    private void SpeedBoost(Vector3 Direction)
    {
        rb.AddForce(Direction * boosterStrength, ForceMode.Impulse);
    }
}