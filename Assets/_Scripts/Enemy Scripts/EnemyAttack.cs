using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public Enemy enemyScript;
    public BoxCollider enemyRightArm;
    private PlayerHelthAndRespawn healthScript;

    private bool hasDealtDamage = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerObj") && !hasDealtDamage)
        {
            Debug.Log("Colliding with PlayerObj");
            healthScript = other.GetComponent<PlayerHelthAndRespawn>();

            if (healthScript != null)
            {
                healthScript.TakeDamage(enemyScript.damageAmount);
                hasDealtDamage = true; // Set the flag to true to prevent further damage
            }
        }
        
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("PlayerObj"))
        {
            hasDealtDamage = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PlayerObj"))
        {
            hasDealtDamage = false;
        }
    }


}
