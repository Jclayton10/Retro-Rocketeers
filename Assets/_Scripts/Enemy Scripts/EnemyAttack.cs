using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    private bool hasDealtDamage = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerObj") && !hasDealtDamage)
        {
            PlayerHelthAndRespawn playerHelth = other.GetComponent<PlayerHelthAndRespawn>();
            playerHelth.TakeDamge(EnemyContoller.enemyContoller.damageAmount);
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
