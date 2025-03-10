using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ConfigureVehicle : MonoBehaviour
{
    void Start()
    {
        Rigidbody rb=GetComponent<Rigidbody>();
		rb.centerOfMass=new Vector3(0, -0.5f, 0);
    }
}
