using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHealth;
    public int damageAmount;
    private int currentHealth;

    // Start is called before the first frame update
    void Start()
    {
        maxHealth = 100;
    }

    // Update is called once per frame
    void Update()
    {


    }

    public void TakeDamage(int damageAmount)
    {
        maxHealth -= damageAmount;

        if (maxHealth <= 0)
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
