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
            EnemyContoller enemy = other.GetComponent<EnemyContoller>();
            Rigidbody rb = other.GetComponentInParent<Rigidbody>();

            if (rb != null && enemy != null && !didHit)
            {
                enemy.TakeDamage(attackDamage);
                enemy.enemyHitSound.volume = GameMaster.Instance.AudioMaster * GameMaster.Instance.AudioSFX;
                enemy.enemyHitSound.Play();

                Vector3 knockbackDirection = (other.transform.position - transform.position).normalized;
                rb.AddForce(knockbackDirection * knockbackForce, ForceMode.Impulse);
                didHit = true;
            }
        }
        else if (other.CompareTag("Resource"))
        {
            ResourceBrain enemy = other.GetComponent<ResourceBrain>();

            if (enemy != null && !didHit)
            {
                enemy.TakeDamage(attackDamage);
                enemy.enemyHitSound.volume = GameMaster.Instance.AudioMaster * GameMaster.Instance.AudioSFX;
                enemy.enemyHitSound.Play();

                didHit = true;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("Resource"))
        {
            didHit = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("Resource"))
        {
            didHit = false;
        }
    }
}
