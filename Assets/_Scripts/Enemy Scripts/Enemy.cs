using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int damageAmount = 10;
    public int currentHealth = 100;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHelthAndRespawn.playerHealth.TakeDamge(damageAmount);
        }
        if (other.CompareTag("Sword"))
        {
            TakeDamage(5);
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
