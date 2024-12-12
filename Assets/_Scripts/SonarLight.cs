using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SonarLight : MonoBehaviour
{
    public GameObject echoLightPrefab;
    private float fadeDuration = 1f;  // Time to fade in/out
    private float maxIntensity = 50f;  // Maximum light intensity

   


    public void ActivateLightEffect(Vector3 position)
    {
        // Instantiate the light at the specified position
        GameObject lightObject = Instantiate(echoLightPrefab, position, Quaternion.identity);

       

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
}
