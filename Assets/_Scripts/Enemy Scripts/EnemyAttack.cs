using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{   private bool hasDealtDamage = false;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerObj") && !hasDealtDamage)
        {
          
            PlayerHelthAndRespawn.playerHealth.TakeDamage(Enemy.enemyScript.damageAmount);
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
