using System;
using UnityEngine;

public class ArtifactScript : MonoBehaviour
{
    [SerializeField] private float spinSpeed;
    private void FixedUpdate()
    {
        transform.Rotate(Vector3.forward, Time.deltaTime * spinSpeed);
    }
    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
    }
}
