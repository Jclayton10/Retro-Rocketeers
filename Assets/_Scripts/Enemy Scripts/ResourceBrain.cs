using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceBrain : MonoBehaviour
{
    [Header("Resource Info")]
    private InventoryManagement inv;

    public ItemClass generated;

    [Header("Enemy Varibles")]
    public AudioSource enemyHitSound;
    public int damageAmount = 10;
    public int currentHealth = 100;

    void Start()
    {
        GameObject inventory = GameObject.Find("Inventory");
        inv = inventory.GetComponent<InventoryManagement>();
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
        enemyHitSound.Stop();
        inv.Add(generated, 1);
        Destroy(gameObject);

    }
}
