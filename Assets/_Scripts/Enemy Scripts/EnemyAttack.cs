using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
  
    EnemyContoller enemyControl;

    
    private void Awake()
    {
        enemyControl = FindFirstObjectByType<EnemyContoller>();
    }
  
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerObj") && enemyControl.isAttaking)
        {
            PlayerHelthAndRespawn playerHelth = other.GetComponent<PlayerHelthAndRespawn>();
            playerHelth.TakeDamge(EnemyContoller.enemyContoller.damageAmount);
            enemyControl.isAttaking = false;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        Wait();
        if (other.CompareTag("PlayerObj"))
        {
            enemyControl.isAttaking = true;
        }
    }
    IEnumerator Wait()
    {
        yield return new WaitForSeconds(2.0f);
    }
}
