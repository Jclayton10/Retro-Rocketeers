using System;
using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{

    public Animator playerAttack;
    public GameObject weaponHoslter;
    public GameObject weaponDraw;

    public int attackDamage = 10;
    public int knockbackForce = 5;

    private bool isWeapomDrawn;
    private bool isWeaponShethed;

  
    

    private void Awake()
    {
        weaponDraw.SetActive(false);
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

                rb.AddForce((-other.transform.forward * knockbackForce), ForceMode.VelocityChange);
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
            weaponDraw.SetActive(true);

            playerAttack.SetTrigger("Withdraw");
        }
        else if (isWeapomDrawn == true && GameMaster.Instance.SheathJustPressed)
        {
            isWeapomDrawn = false;
            isWeaponShethed = true;

            weaponHoslter.SetActive(true);
            weaponDraw.SetActive(false);

            playerAttack.SetTrigger("Sheathing");
        }
        else if (isWeapomDrawn == true && GameMaster.Instance.AttackJustPressed)
        {
            playerAttack.SetTrigger("Attack");
        }
    }


}