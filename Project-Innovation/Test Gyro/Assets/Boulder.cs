using System;
using UnityEngine;

public class Boulder : MonoBehaviour
{
    Rigidbody rb;
    public float breakVelocityThreshold = 5f; // Pas dit aan voor de gewenste snelheid

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Breakable"))
        {
            float verticalSpeed = Mathf.Abs(rb.linearVelocity.y); // Alleen wereldruimte verticale snelheid
            if (verticalSpeed >= breakVelocityThreshold)
            {
                Destroy(other.gameObject);
                Debug.Log($"Broke it with a vertical speed of {verticalSpeed}");
            }
            else
            {
                Debug.Log($"Vertical impact speed {verticalSpeed} too low to break it.");
            }
        }
    }
}