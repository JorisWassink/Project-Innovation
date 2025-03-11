using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScript : MonoBehaviour
{
    public GameObject PauseScreen;
    public GameObject PauseButton;

    public void Pause()
    {
        PauseScreen.SetActive(true);
        PauseButton.SetActive(false);
        Time.timeScale = 0;
    }

    public void Continue()
    {
        PauseScreen.SetActive(false);
        PauseButton.SetActive(true);
        Time.timeScale = 1;
    }
}
