using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public int damageAmount = 10;
    public int currentHealth = 100;
    private int maxHealth;
    public AudioSource enemyHitSound;

    private PlayerHelthAndRespawn healthScript;
    private WaveSpawner waveSpawner;

    private void Awake()
    {
        waveSpawner = FindFirstObjectByType<WaveSpawner>();
    }
    private void Start()
    {
        
        maxHealth = currentHealth;
        if (HealthBarUI.healthBarUI != null)
        {
            HealthBarUI.healthBarUI.SetMaxHealth(maxHealth);
        }
    }

    private void Update()
    {
        if (HealthBarUI.healthBarUI != null)
        {
            HealthBarUI.healthBarUI.SetHealth(currentHealth);
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
        waveSpawner.waves[waveSpawner.currentWaveIndex].enemiesLeft--;
        Destroy(gameObject);
    }
}
