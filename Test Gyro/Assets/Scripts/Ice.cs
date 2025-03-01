using UnityEngine;

public class IceZone : MonoBehaviour {
    private float originalDrag;
    private float iceDrag = 0.001f; // Ice friction
    private Vector3 lastVelocity;

    void OnTriggerEnter(Collider other) {
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb != null) {
            originalDrag = rb.linearDamping;
            rb.linearDamping = iceDrag; // Set drag for ice
            rb.angularDamping = 0;
        }
    }

    void OnTriggerExit(Collider other) {
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb != null) {
            rb.linearDamping = originalDrag; // Reset normal drag
            rb.angularDamping = 0.8f;
        }
    }

    void FixedUpdate() {
        if (transform.GetComponent<Collider>().bounds.Contains(transform.position)) {
            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null) {
                lastVelocity = rb.linearVelocity; // Store current velocity for inertia
                // Apply a slight force to continue momentum
                rb.AddForce(lastVelocity * 0.4f, ForceMode.Acceleration); 
            }
        }
    }
}
