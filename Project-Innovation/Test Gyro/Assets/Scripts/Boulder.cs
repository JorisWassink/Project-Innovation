using System;
using UnityEngine;

public class Boulder : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] private float boosterStrength = 50f;
    

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    

    private void OnCollisionEnter(Collision other)
    {


        if (other.gameObject.CompareTag("SpeedBooster"))
        {
            SpeedBoost(other.transform.forward);
        }
        
        
    }
    

    
    
    private void SpeedBoost(Vector3 Direction)
    {
        rb.AddForce(Direction * boosterStrength, ForceMode.Impulse);
    }
}