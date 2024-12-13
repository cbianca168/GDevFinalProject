using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCSpider : MonoBehaviour
{
    // for idle spider
    public Transform[] points;
    private NavMeshAgent agent;
    private int destPoint = 0;

    // npc target transform
    public Transform goal;
    private bool playerInZone = false;

    // animation
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInZone)
        {
            FollowPlayer();
        }
        else
        {
            // Choose the next destination point when the agent gets
            // close to the current one.
            if (!agent.pathPending && agent.remainingDistance < 0.5f)
                GotoNextPoint();
        }
    }

    void GotoNextPoint()
    {
        // Returns if no points have been set up
        if (points.Length == 0)
            return;

        // Set the agent to go to the currently selected destination.
        agent.destination = points[destPoint].position;
        //animator.SetBool("isWalking", true);

        if (transform.position == points[destPoint].position)
        {
            //animator.SetBool("isWalking", false);
        }

        // This the sample code from the original Unity Manual. 
        // Choose the next point in the array as the destination,
        // cycling to the start if necessary.
        //destPoint = (destPoint + 1) % points.Length;

    }

    void FollowPlayer()
    {
        agent.destination = goal.position;
        animator.SetBool("isWalking", true);

        if (transform.position ==  goal.position)
        {
            animator.SetBool("isWalking", false);
        }
    }

    public void SetPlayerInZone(bool inZone)
    {
        playerInZone = inZone;
    }

}
