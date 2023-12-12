using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAttack : MonoBehaviour
{
    public int attackDamage = 10;
    public int knockbackForce = 5;

    WeaponActivate weaponContorl;

    private void Awake()
    {
        weaponContorl = FindFirstObjectByType<WeaponActivate>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyContoller enemy = other.GetComponent<EnemyContoller>();
            Rigidbody rb = other.GetComponentInParent<Rigidbody>();

            if (rb != null && enemy != null &&enemy.enemySphereCollider.isTrigger&&weaponContorl.isAttacking)
            {
            
                enemy.TakeDamage(attackDamage);
                enemy.enemyHitSound.volume = GameMaster.Instance.AudioMaster * GameMaster.Instance.AudioSFX;
                enemy.enemyHitSound.Play();

            }
        }
        else if (other.CompareTag("Resource"))
        {
            ResourceBrain resb = other.GetComponent<ResourceBrain>();
            EnemyContoller enemy = other.GetComponent<EnemyContoller>();
            if (resb != null && enemy.enemySphereCollider.isTrigger&&weaponContorl.isAttacking)
            {
                resb.TakeDamage(attackDamage);
                resb.enemyHitSound.volume = GameMaster.Instance.AudioMaster * GameMaster.Instance.AudioSFX;
                resb.enemyHitSound.Play();

            }
        }
    }


  
    
}
