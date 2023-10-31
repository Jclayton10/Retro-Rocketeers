using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public int damageAmount; // Adjust the spelling of "damageAmount" for consistency

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();

            if (enemy != null)
            {
                enemy.TakeDamage(damageAmount);
            }

            // Always destroy the arrow upon hitting an enemy
            Destroy(gameObject);
        }
    }
}
