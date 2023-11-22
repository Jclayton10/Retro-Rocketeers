using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int damageAmount = 10;
    public int currentHealth = 100;
    private int maxHealth;

    private EnemyMovment enemyMovement; 
    private PlayerHelthAndRespawn healthScript;
    private void Awake()
    {
   
        enemyMovement = GetComponent<EnemyMovment>();
       


    }
    private void Start()
    {
        maxHealth = currentHealth;
        HealthBarUI.healthBarUI.SetMaxHealth(maxHealth);
    }
    private void Update()
    {
        HealthBarUI.healthBarUI.SetHealth(currentHealth);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerObj"))
        {
            other.isTrigger = true; 

            if (healthScript != null)
            {
                healthScript.TakeDamge(damageAmount);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerObj"))
        {
            other.isTrigger = false; 
        }
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
