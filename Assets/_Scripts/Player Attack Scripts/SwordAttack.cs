using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAttack : MonoBehaviour
{
    public int attackDamage = 10;
    public int knockbackForce = 5;
    private bool didHit = false;
   
   
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemyHealth = other.GetComponent<Enemy>();
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (enemyHealth != null && rb != null&& didHit== false)
            {
                enemyHealth.enemyHitSound.Play();
                enemyHealth.TakeDamage(attackDamage);

                Vector3 knockbackDirection = (other.transform.position - transform.position).normalized;
                rb.AddForce(knockbackDirection * knockbackForce, ForceMode.Impulse);
                didHit = true;
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            didHit = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            didHit = false;
        }
    }
}
