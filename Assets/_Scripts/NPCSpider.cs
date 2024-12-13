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

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GotoNextPoint()
    {
        // Returns if no points have been set up
        if (points.Length == 0)
            return;

        // Set the agent to go to the currently selected destination.
        agent.destination = points[destPoint].position;

        // This the sample code from the original Unity Manual. 
        // Choose the next point in the array as the destination,
        // cycling to the start if necessary.
        //destPoint = (destPoint + 1) % points.Length;

    }

    void FollowPlayer()
    {
        agent.destination = goal.position;
    }

    public void SetPlayerInZone(bool inZone)
    {
        playerInZone = inZone;
    }

}
