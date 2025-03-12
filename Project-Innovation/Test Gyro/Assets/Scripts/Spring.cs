using UnityEngine;

public class Spring : MonoBehaviour
{
    public float bounceForce = 10f; // Pas dit aan voor de gewenste kracht
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("boulder"))
        {
            Debug.Log("Bounce");
            Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                FindObjectOfType<audioManager>().Play("jumpPlant");
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, bounceForce, rb.linearVelocity.z);
                animator.SetTrigger("Bounce");
                Debug.Log("Boulder bounced!");
            }
        }
    }
    
    public void OnAnimationComplete()
    {
        animator.ResetTrigger("Bounce");
    }
}