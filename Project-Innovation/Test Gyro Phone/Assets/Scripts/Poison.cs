using UnityEngine;

public class PoisonZone : MonoBehaviour {
    private float originalDrag;
    private float poisonDrag = 2f; // Poison friction (slow down)
    private float poisonMultiplier = 0.8f; // Poison slow effect
    private bool isInPoisonZone = false;

    void OnTriggerEnter(Collider other) {
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb != null) {
            originalDrag = 0;
            rb.linearDamping = poisonDrag; // Increase drag in the poison zone to slow down the ball
            isInPoisonZone = true;
        }
    }

    void OnTriggerExit(Collider other) {
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb != null) {
            rb.linearDamping = originalDrag; // Reset normal drag
            isInPoisonZone = false;
        }
    }

    void FixedUpdate() {
        if (isInPoisonZone) {
            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null) {
                rb.linearVelocity *= poisonMultiplier; // Gradually slow down the velocity
            }
        }
    }
}
