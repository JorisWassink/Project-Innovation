using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimArrow : MonoBehaviour
{
    public FollowGoalScipt followgoal;
    public TimerScript timer;
    public float timernow;

    public GameObject Arrow;

    [SerializeField]
    private Transform target;
    [SerializeField]
    private Transform arrowPivot;
    [SerializeField]
    private Transform arrowTip;
    [SerializeField]
    private float rotationSpeed = 5f;
    [SerializeField]
    private Vector3 rotationOffset = new Vector3(0, 90, 0);

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Goal 0").transform;
        SetTimer();
        Arrow.GetComponent<Renderer>().enabled = false;
    }

    private void Update()
    {
        if (followgoal.goal1hit == false && timer.currentTime >= timernow + 5)
        {
            Arrow.GetComponent<Renderer>().enabled = true;
            target = GameObject.FindGameObjectWithTag("Goal 1").transform;
        }

        else if (followgoal.goal2hit == false && timer.currentTime >= timernow + 5)
        {
            Arrow.GetComponent<Renderer>().enabled = true;
            target = GameObject.FindGameObjectWithTag("Goal 2").transform;
        }

        else if (followgoal.goal3hit == false && timer.currentTime >= timernow + 5)
        {
            Arrow.GetComponent<Renderer>().enabled = true;
            target = GameObject.FindGameObjectWithTag("Goal 3").transform;
        }

        if (target != null)
        {
            // Calculate the direction to the target
            Vector3 direction = (target.position - arrowPivot.position).normalized;

            // Calculate the look rotation
            Quaternion lookRotation = Quaternion.LookRotation(direction);

            // Apply the rotation offset
            Quaternion offsetRotation = Quaternion.Euler(rotationOffset);
            lookRotation *= offsetRotation;

            // Rotate the arrowPivot to face the target with the offset
            arrowPivot.rotation = Quaternion.Slerp(arrowPivot.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        }
    }

    public void SetTimer()
    {
        timernow = timer.currentTime;
        Debug.Log("timernow = timer.currentTime; (" + timernow + ")");
    }
}

//To Use:
//1. The playable character should have the FollowGoal script
//Fill in other objects
//2. Copy the Arrow with the ArrowTip and apply the AimArrow script to the Arrow
//The UI should have the Timer script
//AimArrow script target is target 1 (arrow will become visible after 5 seconds)
//Fill in other objects
//3. Create triggers with the tags Goal 1, Goal 2 and Goal 3 (no script needed)
//I think it should now work yay