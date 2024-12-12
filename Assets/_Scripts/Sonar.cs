using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sonar : MonoBehaviour
{
    public Material shockwaveMaterial;
    private bool isEffectActive = false;
    private float effectDuration = 1f; // Duration of the shockwave
    private float effectTimer = 0f;

    private SonarLight sonarLight;


    void Update()
    {
        if (isEffectActive)
        {
            effectTimer += Time.deltaTime / effectDuration;

            if (shockwaveMaterial != null)
            {
                shockwaveMaterial.SetFloat("_Effect", effectTimer);
            }

            if (effectTimer >= 1f)
            {
                isEffectActive = false;
                effectTimer = 0f; // Reset for the next trigger
                shockwaveMaterial.SetFloat("_Effect", 0f); // Ensure it's turned off
            }
        }
    }

    public void TriggerSonarEffect()
    {
        if (!isEffectActive)
        {
            isEffectActive = true;
        }

        StartCoroutine(WaitAndDoSomething());
        
    }

    public IEnumerator WaitAndDoSomething()
    {
        // Wait for the specified amount of time
        yield return new WaitForSeconds(1f);

        // After the wait time, execute the action
        sonarLight = GetComponent<SonarLight>();
        sonarLight.ActivateLightEffect(transform.position + transform.forward * 5 + transform.up * 2);

        Debug.Log("Trying to call the activate light effect ");
    }

}
