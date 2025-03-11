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
        switch (other.tag)
        {
           case "Goal 1": 
               GoalReached(diamond1);
               goal1hit = true;
                break;
           case "Goal 2":
               GoalReached(diamond2);
               goal2hit = true;
               break;
           case "Goal 3":
               GoalReached(diamond3);
               goal3hit = true;
               break;
           case "Button":
               changelayoutscript.LayoutChanged();
               break;
        }
    }

    private void GoalReached(GameObject diamond)
    {
        diamond.gameObject.SetActive(false);
        Arrow.GetComponent<Renderer>().enabled = false;
        aimarrow.SetTimer();
        diamond.SetActive(true);
    }
}