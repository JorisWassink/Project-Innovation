using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Finish : MonoBehaviour
{
    public TimerScript timer;
    public GameObject nextLevelScreen;

    public Turn turn;
    public Accelerate accelerate;

    public GameObject PauseButton;

    public GameObject PoliceTrekker;
    public GameObject Arrow;


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            timer.Stop();

            nextLevelScreen.SetActive(true);
            PauseButton.SetActive(false);

            turn.isAlive = false;
            accelerate.isAlive = false;

            Destroy(PoliceTrekker);
            Destroy(Arrow);
        }
    }
}
