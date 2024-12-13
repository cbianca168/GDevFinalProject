using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSpiderHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;


    void Start()
    {
        currentHealth = maxHealth;
    }


    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Enemy Health:" + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Enemy has died!");
        //gameObject.SetActive(false);
        Destroy(gameObject);
    }
}
