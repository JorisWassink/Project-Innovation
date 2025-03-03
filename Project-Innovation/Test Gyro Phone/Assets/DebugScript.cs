using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class DebugScript : MonoBehaviour
{
    private TMPro.TextMeshProUGUI textMesh;
    private void Start()
    {
        textMesh = GetComponent<TMPro.TextMeshProUGUI>();
        Input.gyro.enabled = true; // Enable the gyroscope if it's supported
        
        

    }
    
    private void OnEnable()
    {
        InputSystem.EnableDevice(UnityEngine.InputSystem.Gyroscope.current);
    }

    private void Update()
    {
        if (SystemInfo.supportsGyroscope)
        {
            textMesh.text = Input.gyro.attitude.ToString();
        }
        else
        {
            textMesh.text = "Gyroscope not available on this device.";
        }

        
    }
}