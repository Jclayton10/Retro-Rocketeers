using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy : MonoBehaviour
{

    public int damageAmount = 10;
    public int currentHealth = 100;
    private int maxHealth;


    public static Enemy enemyScript;
    public AudioSource enemyHitSound;



    private WaveSpawner waveSpawner;

    private void Awake()
    {

        waveSpawner = FindFirstObjectByType<WaveSpawner>();
    }

    private void Start()
    {
        enemyScript = this;
        maxHealth = currentHealth;

        // Get the EnemyHealthBarUI component dynamically
        EnemyHealthBarUI enemyHealthBarUI = FindFirstObjectByType<EnemyHealthBarUI>();

        if (enemyHealthBarUI != null)
        {
            enemyHealthBarUI.SetMaxHealth(maxHealth);
        }
    }

    private void Update()
    {
        // Get the EnemyHealthBarUI component dynamically
        EnemyHealthBarUI enemyHealthBarUI = FindFirstObjectByType<EnemyHealthBarUI>();

        if (enemyHealthBarUI != null)
        {
            enemyHealthBarUI.SetHealth(currentHealth);
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
        AchievementManager.enemyAchCount += 1;

        // Check if the script is not null before accessing it
        if (waveSpawner != null)
        {
            // Access the waveSpawner object
            // waveSpawner.waves[waveSpawner.currentWaveIndex].enemiesLeft--;
        }

        Destroy(gameObject);

    }
}
