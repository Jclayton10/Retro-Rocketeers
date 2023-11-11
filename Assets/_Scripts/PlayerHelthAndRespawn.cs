using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHelthAndRespawn : MonoBehaviour
{
    public static PlayerHelthAndRespawn playerHealth;
    public int maxHealth = 100;
    public int currentHealth = 100;
    [SerializeField] Transform playerLocation;

    public void TakeDamge(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Method to handle player death (you can implement your own logic)
    private void Die()
    {
        currentHealth = maxHealth;
        transform.parent.transform.position = playerLocation.position;
    }
}
