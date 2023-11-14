using System;
using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{

    public Animator playerAttack;
    public GameObject weaponHoslter;


    public int attackDamage = 10;
    public int knockbackForce = 5;

    private bool isWeapomDrawn;
    private bool isWeaponShethed;

    private MeshRenderer swordGripRenderer;
    private MeshCollider swordGripMeshCollider;
  
    

    private void Awake()
    {
        isWeapomDrawn = false;
        isWeaponShethed = true;
        swordGripRenderer = gameObject.GetComponentInChildren<MeshRenderer>();
        swordGripRenderer.enabled = false;
        swordGripMeshCollider = gameObject.GetComponent<MeshCollider>();
        swordGripMeshCollider.enabled = false;
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
           

            swordGripRenderer = gameObject.GetComponentInChildren<MeshRenderer>();
            swordGripRenderer.enabled = true;
            swordGripMeshCollider = gameObject.GetComponent<MeshCollider>();
            swordGripMeshCollider.enabled = true;

            playerAttack.SetTrigger("Withdraw");
        }
        else if (isWeapomDrawn == true && GameMaster.Instance.SheathJustPressed)
        {
            playerAttack.SetTrigger("Sheathing");

            isWeapomDrawn = false;
            isWeaponShethed = true;

            weaponHoslter.SetActive(true);
          

            swordGripRenderer = gameObject.GetComponentInChildren<MeshRenderer>();
            swordGripRenderer.enabled = false;
            swordGripMeshCollider = gameObject.GetComponent<MeshCollider>();
            swordGripMeshCollider.enabled = false;
        }
        else if (isWeapomDrawn == true && GameMaster.Instance.AttackJustPressed)
        {
            playerAttack.SetTrigger("Attack");
        }
    }


}