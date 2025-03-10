using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SimplePlayerControls : MonoBehaviour
{
	public float speed=10;

    Rigidbody rb;
    Vector3 startPosition;

    void Start()
    {
		rb = GetComponent<Rigidbody>();
		startPosition = transform.position;
    }

    void FixedUpdate()
    {
         Vector3 moveVector = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

         rb.AddForce(moveVector * speed);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Car")){
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    void ResetPosition() {
        if (rb != null) {
            rb.position = startPosition;
        } else {
            Debug.Log("Warning: no rigidbody attached to player");
        }
    }
}
