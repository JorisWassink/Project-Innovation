using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Turn : MonoBehaviour
{
    public float turnSpeed = 100f;

    public bool isAlive = true;
   
    void Start()
    {
        isAlive = true;
    }

    void Update()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        if (currentSceneName != "Welcome" && currentSceneName != "Win")
        {
            if (isAlive == true)
            {
                transform.Rotate(Vector3.up, turnSpeed * Time.deltaTime * Input.GetAxis("Horizontal"));
            }
        }
    }
}
