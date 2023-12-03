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

    public void TakeDamage(int amount)
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

        // Deactivate the player or perform other death-related logic
        gameObject.SetActive(false);

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
        healthBarUI.SetHealth(maxHealth);
    }
}
