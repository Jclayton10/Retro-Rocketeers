using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int damageAmount = 10;
    public int currentHealth = 100;
    private int maxHealth;

    public CapsuleCollider playerCollider;
    private EnemyMovment enemyMovement; // Reference to the EnemyMovment script

    public HealthBarUI healthBarUI;

    [HideInInspector]
    public PlayerHelthAndRespawn healthSpript;
    private void Awake()
    {
        // Get a reference to the EnemyMovment script attached to the same game object
        enemyMovement = GetComponent<EnemyMovment>();
        GameObject gm = GameObject.Find("PlayerObj");
        healthSpript = gm.GetComponent<PlayerHelthAndRespawn>();


    }
    private void Start()
    {
        maxHealth = currentHealth;
        healthBarUI.SetMaxHealth(maxHealth);
    }
    private void Update()
    {
        healthBarUI.SetHealth(currentHealth);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider == playerCollider) 
        {
            if (playerCollider != null)
            {
               
                if (healthSpript != null)
                {
                    healthSpript.TakeDamge(damageAmount);
                    playerCollider.isTrigger = true;
                    
                }
            }
        }

       
    }
    private void OnCollisionExit(Collision collision)
    {

        if (collision.collider == playerCollider)
        {
            if (playerCollider != null)
            {
               
                if (healthSpript != null)
                {
                    healthSpript.TakeDamge(damageAmount);
                    playerCollider.isTrigger = false;
                }
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
