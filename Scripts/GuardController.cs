/* Copyright by: Cory Wolf */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class GuardController : MonoBehaviour
{
    private Animator anim;
    private NavMeshAgent agent;
    public Transform[] patrolPoints;
    private int currentPoint;
    private bool startDelay;
    public float delay;
    private float currentDelay;
    private int lastPoint;
    private bool disablePoints;
    private float currentDistraction;
    public float distractionDelay;
    
    void Start()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        anim.SetBool("Walking", true);

        ChangePos();
        InvokeRepeating("UpdateTarget", 0f, 1f);

        currentDelay = delay;
        currentDistraction = distractionDelay;
    }

    
    void UpdateTarget()
    {
        if (Vector3.Distance(transform.position, patrolPoints[currentPoint].position) <= 0.5f && !disablePoints)
        {
            //startDelay = true;
            //Debug.Log(currentPoint);
            ChangePos();
        }

    }

    private void Update()
    {
        /*if(startDelay)
        {
            anim.SetBool("Walking", false);
            currentDelay -= Time.deltaTime;
            if(currentDelay <= 0)
            {
                anim.SetBool("Walking", true);
                ChangePos();
                currentDelay = delay;
                startDelay = false;
            }
        }*/

        if(disablePoints)
        {
            currentDistraction -= Time.deltaTime;
            if(currentDistraction <= 0)
            {
                currentDistraction = distractionDelay;
                disablePoints = false;
                ChangePos();
            }
        }
    }

    void ChangePos()
    {
        currentPoint++;
        if (currentPoint >= patrolPoints.Length)
            currentPoint = 0;

        agent.SetDestination(patrolPoints[currentPoint].position);
    }

    public void ChangeDestination(Transform location)
    {
        disablePoints = true;
        agent.SetDestination(location.position);
    }
}