using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectedWood : MonoBehaviour
{
    public AudioSource collectSound;
    private bool isCollected = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerObj") && !isCollected)
        {
            collectSound.Play();
            isCollected = true; // Set the flag to true to prevent multiple collections.
            Destroy(gameObject);

            
            /*GlobalAchivments globalAchivments = FindFirstObjectByType<GlobalAchivments>();

            if (globalAchivments != null)
            {
                // Increment the achievement count and trigger the achievement
                globalAchivments.CallAchivmentByAchCode(1); // Pass the achievement code you want to trigger
                //globalAchivments.CallAchivmentByAchCode(2);
            }
            */
        }
    }
}
