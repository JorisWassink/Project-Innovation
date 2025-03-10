using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SimpleCarMovement : MonoBehaviour
{
	public float speed=1;	

	Rigidbody rb;

    void Start()
    {
		rb=GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
		Vector3 moveVector = transform.forward;

		rb.linearVelocity = moveVector * speed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Trigger"))
        {
            Destroy(gameObject);
        }
    }
}
