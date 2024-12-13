using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SonarLight : MonoBehaviour
{
    public GameObject echoLightPrefab;
    private float fadeDuration = 1f;  // Time to fade in/out
    private float maxIntensity = 50f;  // Maximum light intensity

    // damaging effects
    public int damageAmount = 100;      // Damage dealt to enemies
    public float lightDuration = 3f;     // Time before light destroys itself
    public Light pointLight;             // Reference to the Light component
    private float damageCheckInterval = 0.2f; // How often to check for enemies in range
    private float nextDamageTime;

   

    public void ActivateLightEffect(Vector3 position)
    {
        // Instantiate the light at the specified position
        GameObject lightObject = Instantiate(echoLightPrefab, position, Quaternion.identity);

        if (lightObject != null )
        {
            // Periodically deal damage to enemies in range
            if (Time.time >= nextDamageTime)
            {
                DamageToEnemies();
                nextDamageTime = Time.time + damageCheckInterval;
                Debug.Log("Calling damage to enemies function");
            }
        }
       

        // Start the fade-in and fade-out effect coroutine
        StartCoroutine(FadeLightInOut(lightObject));
    }




    private IEnumerator FadeLightInOut(GameObject lightObject)
    {
        Light sonarLightComp = lightObject.GetComponent<Light>();

        if (sonarLightComp != null)
        {
            // Fade in
            float currentTime = 0f;
            while (currentTime < fadeDuration)
            {
                sonarLightComp.intensity = Mathf.Lerp(0, maxIntensity, currentTime / fadeDuration);
                currentTime += Time.deltaTime;
                yield return null;
            }

            sonarLightComp.intensity = maxIntensity;

            // Wait for a bit (optional: you can change this time)
            yield return new WaitForSeconds(1f);

            // Fade out
            currentTime = 0f;
            while (currentTime < fadeDuration)
            {
                sonarLightComp.intensity = Mathf.Lerp(maxIntensity, 0, currentTime / fadeDuration);
                currentTime += Time.deltaTime;
                yield return null;
            }

            sonarLightComp.intensity = 0;

            // Destroy the light after fading out
            Destroy(lightObject);
        }
    }

    void DamageToEnemies()
    {
        // Use the light's position and range
        Vector3 lightPosition = transform.position;
        float lightRange = pointLight.range;

        // Find all colliders in the light's range
        Collider[] hitColliders = Physics.OverlapSphere(lightPosition, lightRange);

        foreach (Collider hitCollider in hitColliders)
        {
            // Check for specific enemy tags
            if (hitCollider.CompareTag("Enemy"))
            {
                NPCSpiderHealth enemyHealth = hitCollider.GetComponent<NPCSpiderHealth>();
                if (enemyHealth != null)
                {
                    // Apply damage
                    enemyHealth.TakeDamage(damageAmount);
                    Debug.Log($"{hitCollider.name} took {damageAmount} damage from the sonar light!");
                }
            }
        }
    }


}
