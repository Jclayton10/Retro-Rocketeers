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
        ResetDamageFlag();
    }

    // You can reset the hasDealtDamage flag if needed (e.g., if the enemy's arm retracts)
    public void ResetDamageFlag()
    {
        hasDealtDamage = false;
    }
}
