using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CollectCube : MonoBehaviour
{
    public AudioSource collectSound;
    private bool isCollected = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerObj")&& !isCollected) 
        {
            collectSound.Play();
            AchievementManager.enemyAchCount += 1;
            isCollected = true;
            Destroy(gameObject);
        }
    }
}
