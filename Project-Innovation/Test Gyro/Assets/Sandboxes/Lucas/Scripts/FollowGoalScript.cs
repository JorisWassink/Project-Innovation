using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class FollowGoalScipt : MonoBehaviour
{
    public AimArrow aimarrow;
    public GameObject Arrow;
    public ChangeLayout changelayoutscript;

    public bool goal1hit = false;
    public bool goal2hit = false;
    public bool goal3hit = false;
    public bool goal4hit = false;

    public GameObject diamond1;
    public GameObject diamond2;
    public GameObject diamond3;
    public GameObject diamond4;

    void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
           case "Goal 0": 
               GoalReached(diamond1, other.gameObject);
               goal1hit = true;
                break;
           case "Goal 1":
               GoalReached(diamond2, other.gameObject);
               goal2hit = true;
               break;
           case "Goal 2":
               GoalReached(diamond3, other.gameObject);
               goal3hit = true;
               break;
           case "Goal 3":
               GoalReached(diamond4, other.gameObject);
               goal4hit = true;
               Win();
               break;
           case "Button":
               changelayoutscript.LayoutChanged();
               break;
        }
    }

    private void GoalReached(GameObject diamond, GameObject goal)
    {
        Arrow.GetComponent<Renderer>().enabled = false;
        aimarrow.SetTimer();
        diamond.SetActive(true);
        Destroy(goal);
    }

    private void Win()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}