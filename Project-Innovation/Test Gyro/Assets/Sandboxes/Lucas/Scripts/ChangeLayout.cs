using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeLayout : MonoBehaviour
{
    public TimerScript timer;
    public float timernow;

    public GameObject LayoutText;

    public void LayoutChanged()
    {
        SetTimer();
        LayoutText.SetActive(true);
    }

    private void Update()
    {
        if (timer.currentTime >= timernow + 7.5)
        {
            LayoutText.SetActive(false);
        }
    }

    public void SetTimer()
    {
        timernow = timer.currentTime;
        Debug.Log("ChangeLayout: timernow = timer.currentTime; (" + timernow + ")");
    }
}