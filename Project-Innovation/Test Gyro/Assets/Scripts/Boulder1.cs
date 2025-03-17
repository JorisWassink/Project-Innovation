using System;
using UnityEngine;

public class KeyboardBoulder : MonoBehaviour
{
    [SerializeField] private float boosterStrength = 500f;
    [SerializeField] private float boulderSpeed = 5f;
    [SerializeField] private float tiltSmoothing = 5f;
    private float normalDrag;
    [SerializeField] private Transform cameraTransform;
    private Vector3 targetForce;
    private Rigidbody rb;

    private float iceDrag = 0.1f;
    private float mudDrag = 3f;
    private float currentDrag;
    private float boosterForce = 1;
    private float baseSpeed;

    void Start()
    {
        baseSpeed = boulderSpeed;
        rb = GetComponent<Rigidbody>();
        normalDrag = rb.linearDamping;
        currentDrag = normalDrag;
    }

    private void Update()
    {
        if (cameraTransform == null) return;

        Vector3 flatForward = new Vector3(cameraTransform.forward.x, 0, cameraTransform.forward.z).normalized;
        Vector3 flatRight = new Vector3(cameraTransform.right.x, 0, cameraTransform.right.z).normalized;

        boulderSpeed *= boosterForce;
        if (boosterForce > 1) boosterForce -= 0.1f;
        boulderSpeed = Mathf.Lerp(boulderSpeed, baseSpeed, Time.deltaTime * 2f);

        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");
        Vector3 keyboardForce = (flatForward * moveY + flatRight * moveX) * boulderSpeed;
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpeedBoost(keyboardForce);
        }

        targetForce = Vector3.Lerp(targetForce, keyboardForce, tiltSmoothing * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        if (rb == null) return;

        Vector3 newVelocity = targetForce;

        if (currentDrag == iceDrag)
        {
            newVelocity *= 1f / (1f + Time.deltaTime * 5f);
        }
        else if (currentDrag == mudDrag)
        {
            newVelocity *= 0.5f;
        }

        newVelocity.y = rb.linearVelocity.y;
        rb.linearVelocity = newVelocity;

//        Debug.Log($"Keyboard Force Applied: {newVelocity}");
    }

    private void SpeedBoost(Vector3 direction)
    {
        boosterForce = boosterStrength;
        Debug.Log(boosterForce);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ice"))
        {
            currentDrag = iceDrag;
        }
        else if (other.gameObject.CompareTag("Mud"))
        {
            currentDrag = mudDrag;
        }
        if (other.gameObject.CompareTag("SpeedBooster"))
        {
            SpeedBoost(rb.linearVelocity.normalized);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Ice") || other.gameObject.CompareTag("Mud"))
        {
            currentDrag = normalDrag;
        }
    }
}
