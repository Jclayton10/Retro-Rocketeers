using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
 
    public int damageAmount = 10;
    public int currentHealth = 100;
    private int maxHealth;

    private PlayerHelthAndRespawn healthScript;
    
    private void Start()
    {
        maxHealth = currentHealth;
        HealthBarUI.healthBarUI.SetMaxHealth(maxHealth);
    }

    private void Update()
    {
        HealthBarUI.healthBarUI.SetHealth(currentHealth);
    }

   

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
