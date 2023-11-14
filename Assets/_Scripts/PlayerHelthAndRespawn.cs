using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHelthAndRespawn : MonoBehaviour
{
    public static PlayerHelthAndRespawn playerHealth;
    public int maxHealth = 100;
    public int currentHealth;

    public HealthBarUI healthBarUI;

    [SerializeField] Transform playerLocation;

    private void Start()
    {
        currentHealth = maxHealth;
        healthBarUI.SetMaxHealth(maxHealth);
    }
    public void TakeDamge(int amount)
    {
        currentHealth -= amount;
        healthBarUI.SetHealth(currentHealth);
        if (currentHealth <= 0)
        {
            Die();
        }
    }

 
    private void Die()
    {
        currentHealth = maxHealth;
        transform.parent.transform.position = playerLocation.position;
    }
}
