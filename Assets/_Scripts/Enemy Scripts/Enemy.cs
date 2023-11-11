using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int damageAmount = 10;
    public int currentHealth = 100;

    public CapsuleCollider playerCollider;
    public SphereCollider sphereCollider;

    private EnemyMovment enemyMovement; // Reference to the EnemyMovment script

    [HideInInspector]
    public PlayerHelthAndRespawn healthSpript;
    private void Awake()
    {
        // Get a reference to the EnemyMovment script attached to the same game object
        enemyMovement = GetComponent<EnemyMovment>();
        GameObject gm = GameObject.Find("PlayerObj");
        healthSpript = gm.GetComponent<PlayerHelthAndRespawn>();


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

        if (collision.collider == sphereCollider) 
        {
            sphereCollider.isTrigger=true;
            enemyMovement.anim.SetBool("isMoving", false);
            enemyMovement.anim.speed = 0;
            enemyMovement.agent.isStopped = true;
             
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
        if(collision.collider == sphereCollider)
        {
            sphereCollider.isTrigger = false;
            enemyMovement.anim.SetBool("isMoving", true);
            enemyMovement.anim.speed = 0;
            enemyMovement.agent.isStopped = false;
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
