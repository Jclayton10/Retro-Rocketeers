using System;
using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public PlayerAttack playerAttackScript;

    public Animator playerAttack;
    public GameObject weaponHoslter;
    public GameObject weaponGrip;


    public int attackDamage = 10;
    public int knockbackForce = 5;

    private bool isWeapomDrawn;
    private bool isWeaponShethed;





    private void Awake()
    {
        playerAttackScript = FindFirstObjectByType<PlayerAttack>();  
        weaponGrip.SetActive(false);
        isWeapomDrawn = false;
        isWeaponShethed = true;
        
    }

    private void Update()
    {
        PlayAnimations();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemyHealth = other.GetComponent<Enemy>();
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (enemyHealth != null && rb != null)
            {
                // Apply the damage to the enemy
                enemyHealth.TakeDamage(attackDamage);
                Debug.Log("EnemyHealth: " + enemyHealth.ToString());
                // Apply combined forces only if force hasn't been applied yet

                Vector3 knockbackDirection = (other.transform.position - transform.position).normalized;
                rb.AddForce(knockbackDirection * knockbackForce, ForceMode.Impulse);
            }
        }
    }

    void PlayAnimations()
    {
        if (isWeaponShethed == true && GameMaster.Instance.SheathJustPressed) // Change the input key as needed
        {
            Debug.Log("Unsheating!");
            isWeapomDrawn = true;
            isWeaponShethed = false;

            weaponHoslter.SetActive(false);


            
            

            playerAttack.SetTrigger("Withdraw");
        }
        else if (isWeapomDrawn == true && GameMaster.Instance.SheathJustPressed)
        {
            playerAttack.SetTrigger("Sheathing");

            isWeapomDrawn = false;
            isWeaponShethed = true;

            weaponHoslter.SetActive(true);


            
        }
        else if (isWeapomDrawn == true && GameMaster.Instance.AttackJustPressed)
        {
            playerAttack.SetTrigger("Attack");
        }
    }


}