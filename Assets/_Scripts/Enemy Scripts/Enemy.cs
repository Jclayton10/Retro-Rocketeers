using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int damageAmount = 10;
    public int currentHealth = 100;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHelthAndRespawn playerScript = other.GetComponent<PlayerHelthAndRespawn>();

            if (playerScript != null)
            {
                // Access the player's CapsuleCollider and set it as a trigger
                CapsuleCollider playerCollider = playerScript.GetComponent<CapsuleCollider>();
                if (playerCollider != null)
                {
                    playerCollider.isTrigger = true;
                }

                // Apply damage to the player
                playerScript.TakeDamge(damageAmount);
            }
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
        // Handle enemy death logic here
        // For example, you can play death animations, give rewards, or destroy the game object
        Destroy(gameObject);
    }
}
