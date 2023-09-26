using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public Animator playerAnimator;
    public GameObject weapon; // Assign your weapon GameObject in the Inspector
    public int attackDamage = 10;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0)) // Change the input key as needed
        {
            Attack();
        }
    }

    void Attack()
    {
        // Play the attack animation
        playerAnimator.SetTrigger("Attack");

       
       // weapon.SetActive(true);

        // Get the Box Collider of the weapon
        BoxCollider weaponCollider = weapon.GetComponent<BoxCollider>();

        // Check for collisions using the Box Collider
        Collider[] colliders = Physics.OverlapBox(weaponCollider.bounds.center, weaponCollider.bounds.extents, weapon.transform.rotation);

        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                // Deal damage to the enemy
                EnemyMovment enemy = collider.GetComponent<EnemyMovment>();
                if (enemy != null)
                {
                    enemy.TakeDamage(attackDamage);
                }
            }
        }

        // Disable the weapon GameObject after the attack animation
        //weapon.SetActive(false);
    }
}
