using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHelthAndRespawn : MonoBehaviour
{
    public static PlayerHelthAndRespawn playerHealth;
    public int maxHealth = 100;
    public int currentHealth;

    

    [SerializeField] Transform playerLocation;
    private void Awake()
    {
        playerHealth = this;
    }
    private void Start()
    {
        currentHealth = maxHealth;
        HealthBarUI.healthBarUI.SetMaxHealth(maxHealth);
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        HealthBarUI.healthBarUI.SetHealth(currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        currentHealth = maxHealth;

       

        // Reset the player position after a short delay (you can adjust the delay)
        StartCoroutine(RespawnAfterDelay());
    }

    private IEnumerator RespawnAfterDelay()
    {
        yield return new WaitForSeconds(2f); // Adjust the delay as needed

        // Reactivate the player
        gameObject.SetActive(true);

        // Reset the player position
        transform.parent.transform.position = playerLocation.position;

        // Reset the health bar
        HealthBarUI.healthBarUI.SetHealth(maxHealth);
    }
}
