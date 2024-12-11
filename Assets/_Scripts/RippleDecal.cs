using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class RippleDecal : MonoBehaviour
{
    public GameObject decalPrefab; // Assign your decal prefab here
    public float decalDuration = 1f; // How long the decal should stay
    private float expandSpeed = 4f; // How fast it expands
    //[SerializeField] private float fadeDuration = 1.0f;


    public void OnStep()
    {
        // Instantiate the decal at the character's feet
        GameObject decal = Instantiate(decalPrefab, transform.position, Quaternion.Euler(90,0,0));
        StartCoroutine(HandleDecal(decal));
        
    }

    private IEnumerator HandleDecal(GameObject decal)
    {
        DecalProjector projector = decal.GetComponent<DecalProjector>();
        float currentSize = 0.1f; // Initial size

        while (currentSize < 10f) // Set your max size here
        {
            
            currentSize += expandSpeed * Time.deltaTime;
            projector.size = new Vector3(currentSize, currentSize, currentSize); // Expand decal

            // Get the material instance from the decal object
            //Material decalInstance = decal.GetComponent<Material>();
            //StartFading(decal);

            yield return null;
        }

        // Wait for the duration, then destroy
        //yield return new WaitForSeconds(decalDuration);
        Destroy(decal);
    }

    /*private void StartFading(GameObject decal)
    {
        StartCoroutine(FadeOutDecal(decal));
    }

    private IEnumerator FadeOutDecal(GameObject decal)
    {
        // Get the material instance from the decal object
        Material decalInstance = decal.GetComponent().material;
        float elapsed = 0f;

        // Store the original intensity or fade value
        float originalFadeValue = decalInstance.GetFloat("_Fade"); // Replace "_Fade" with the correct shader property
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float fade = Mathf.Lerp(originalFadeValue, 0, elapsed / fadeDuration);
            decalInstance.SetFloat("_Fade", fade);
            yield return null;
        }

       // Destroy(decal); // Destroy the decal after it’s fully faded
    }*/

}
