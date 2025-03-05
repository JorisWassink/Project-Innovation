using System;
using UnityEngine;

public class Boulder : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] private float breakVelocityThreshold = 5f;
    [SerializeField] private float boosterStrength = 50f;
    

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Breakable"))
        {
           // BreakWall(other);
        }

        if (other.gameObject.CompareTag("SpeedBooster"))
        {
            SpeedBoost(other.transform.forward);
        }
        
        
    }
    
    private void BreakWall(Collision other)
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
    
    
    private void SpeedBoost(Vector3 Direction)
    {
        rb.AddForce(Direction * boosterStrength, ForceMode.Impulse);
    }
}