using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class FollowGoalScipt : MonoBehaviour
{
    public AimArrow aimarrow;
    public GameObject Arrow;
    public ChangeLayout changelayoutscript;

    public bool goal1hit = false;
    public bool goal2hit = false;
    public bool goal3hit = false;

    public GameObject diamond1;
    public GameObject diamond2;
    public GameObject diamond3;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Goal 1"))
        {
            goal1hit = true;
            other.gameObject.SetActive(false);
            Arrow.GetComponent<Renderer>().enabled = false;
            aimarrow.SetTimer();
            Debug.Log("goal1hit");
            diamond1.SetActive(true);
        }
        else if (other.CompareTag("Goal 2"))
        {
            goal2hit = true;
            other.gameObject.SetActive(false);
            Arrow.GetComponent<Renderer>().enabled = false;
            aimarrow.SetTimer();
            Debug.Log("goal2hit");
            diamond2.SetActive(true);
        }
        else if (other.CompareTag("Goal 3"))
        {
            goal3hit = true;
            other.gameObject.SetActive(false);
            Arrow.GetComponent<Renderer>().enabled = false;
            aimarrow.SetTimer();
            Debug.Log("goal3hit");
            diamond3.SetActive(true);
        }
        else if (other.CompareTag("Button"))
        {
            changelayoutscript.LayoutChanged();
        }
    }
}