using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Damage : MonoBehaviour
{
    public GameObject gameOverScreen;
    public GameObject PauseButton;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Damage"))
        {
            Destroy(gameObject);
            if (gameOverScreen != null)
            {
                gameOverScreen.SetActive(true);
            }

            if (PauseButton != null)
            {
                PauseButton.SetActive(false);
            }
        }
    }
}
