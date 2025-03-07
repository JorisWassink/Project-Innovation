using UnityEngine;

public class Spring : MonoBehaviour
{
    public float bounceForce = 10f; // Pas dit aan voor de gewenste kracht

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("boulder"))
        {
            Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, bounceForce, rb.linearVelocity.z);
                Debug.Log("Boulder bounced!");
            }
        }
    }
}