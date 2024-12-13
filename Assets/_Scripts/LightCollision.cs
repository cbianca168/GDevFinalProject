using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightCollision : MonoBehaviour
{
    public GameObject collisionPrefab;
    
    
    public void ActivateCollision(Vector3 position)
    {
        // Instantiate the light at the specified position
        GameObject collisionObject = Instantiate(collisionPrefab, position, Quaternion.identity);

        // Start the fade-in and fade-out effect coroutine
        StartCoroutine(FadeCollisionInOut(collisionObject));
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            NPCSpiderHealth spiderHealth = FindObjectOfType<NPCSpiderHealth>();
            spiderHealth.TakeDamage(100);
        }
    }

    private IEnumerator FadeCollisionInOut(GameObject collisionObject)
    {
        
            yield return new WaitForSeconds(3f);

            // Destroy the light after fading out
            Destroy(collisionObject);
       
    }


}
