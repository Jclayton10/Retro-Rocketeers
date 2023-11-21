using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public int damageAmount = 10;
    public int currentHealth = 100;
    private int maxHealth;

    private EnemyMovment enemyMovement; // Reference to the EnemyMovment script
    public PlayerHelthAndRespawn healthScript;

    private CapsuleCollider playerCollider;
    public HealthBarUI enemyHealthBar;
    private void Awake()
    {
       
        enemyMovement = GetComponent<EnemyMovment>();
        enemyHealthBar = GetComponentInChildren<HealthBarUI>();
        GameObject playerObj = GameObject.Find("PlayerObj");

        // Ensure that the player object and its components are found before using them
        if (playerObj != null)
        {
            healthScript = playerObj.GetComponent<PlayerHelthAndRespawn>();
            playerCollider = playerObj.GetComponent<CapsuleCollider>();
        }
        else
        {
            Debug.LogError("Player object not found!");
        }
    }

    private void Start()
    {
        maxHealth = currentHealth;
        enemyHealthBar.SetMaxHealth(maxHealth);
    }

    private void Update()
    {
        enemyHealthBar.SetHealth(currentHealth);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider == playerCollider)
        {
            Debug.Log("Enemy collided with the player!");
            healthScript.TakeDamge(damageAmount);
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
