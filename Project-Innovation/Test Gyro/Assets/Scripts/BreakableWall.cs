using System.Collections;
using UnityEngine;

public class BreakableWall : MonoBehaviour
{
    [SerializeField] private GameObject wall;
    [SerializeField] private ParticleSystem breakParticle;
    [SerializeField] private float breakVelocityThreshold = 5f; // Pas de drempel aan indien nodig

    private void OnCollisionEnter(Collision collision)
    {
        Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            float verticalSpeed = Mathf.Abs(rb.linearVelocity.x) + Mathf.Abs(rb.linearVelocity.z); // Alleen de wereldruimte verticale snelheid
            if (verticalSpeed >= breakVelocityThreshold)
            {
                StartCoroutine(Break());
                Debug.Log($"Wall broke due to vertical speed of {verticalSpeed}");
                FindFirstObjectByType<audioManager>().Play("breakWall");
            }
            else
            {
//                Debug.Log($"Vertical impact speed {verticalSpeed} too low to break the wall.");
            }
        }
    }

    IEnumerator Break()
    {
        Destroy(wall); 
        breakParticle.Play();
        yield return new WaitForSeconds(breakParticle.main.duration);
        Destroy(gameObject);
    }
}
