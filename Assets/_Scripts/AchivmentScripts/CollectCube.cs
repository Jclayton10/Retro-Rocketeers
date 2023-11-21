using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectCube : MonoBehaviour
{ 
    public AudioSource collectSound;
    private bool isCollected = false;




    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerObj") && !isCollected)
        {
            GlobalAchivments.ach01Count += 1;
            collectSound.Play();
            isCollected = true; // Set the flag to true to prevent multiple collections.
            Destroy(gameObject);
        }
    }
}
