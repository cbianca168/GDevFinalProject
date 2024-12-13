using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderZoneTrigger : MonoBehaviour
{
    public List<NPCSpider> npcs = new List<NPCSpider>(); // List to hold multiple NPCs


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Notify all NPCs and their gun managers
            foreach (NPCSpider npc in npcs)
            {
                npc.SetPlayerInZone(true);
            }

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Notify all NPCs and their gun managers
            foreach (NPCSpider npc in npcs)
            {
                npc.SetPlayerInZone(false);
            }

            
        }
    }
}
