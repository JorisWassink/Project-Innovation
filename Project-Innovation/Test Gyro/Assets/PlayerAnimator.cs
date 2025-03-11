using System;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private Rigidbody ballRb;
    [SerializeField] private float yOffset;
    [SerializeField] private float animationSpeedCap;
    [SerializeField] private float animationSpeedFactor;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        transform.position = ballRb.position;
        var targetPos = transform.position;
        targetPos.y = ballRb.position.y + yOffset;
        transform.position = targetPos;
        
        var ballSpeed = ballRb.linearVelocity.magnitude;
        
        if (ballSpeed > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(new Vector3(ballRb.linearVelocity.x, 0, ballRb.linearVelocity.z));
            transform.rotation = targetRotation;
        }
        
        ballSpeed *= animationSpeedFactor;

        if (ballSpeed  < animationSpeedCap)
        {
            animator.speed = ballSpeed;
        }
        else
        {
            animator.speed = animationSpeedCap;
        }
    }
}
