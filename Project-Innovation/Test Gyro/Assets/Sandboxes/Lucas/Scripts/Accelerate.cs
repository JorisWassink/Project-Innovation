using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Accelerate : MonoBehaviour
{
    private Rigidbody rb;

    public float accelaration = 80f;
    public float groundFriction = 0.5f;
    public float groundFrictionElse = 0.8f;

    public bool isAlive = true;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        isAlive = true;
    }

    void FixedUpdate()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        if (currentSceneName != "Welcome" && currentSceneName != "Win")
        {
            if (isAlive == true)
            {
                if (IsGrounded())
                {
                    rb.AddForce(transform.forward * accelaration * Input.GetAxis("Vertical"));
                }
            }
        }
    }

    public bool IsGrounded()
    {
        Vector3 origin = transform.position;
        Vector3 direction = Vector3.down;
        float distance = 1.1f;

        Debug.DrawLine(transform.position, transform.position + Vector3.down * 1.1f, Color.red);

        RaycastHit hitInfo;

        if(Physics.Raycast(origin, direction, out hitInfo, distance))
        {
            if (!hitInfo.collider.gameObject.CompareTag("Road"))
            {
                rb.linearVelocity *= groundFrictionElse;
            }
            rb.linearVelocity *= groundFriction;
            return true;
        }
        return false;
    }
}
