using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHealth = 100;
    public int damageAmount;
    private int currentHealth;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {


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
