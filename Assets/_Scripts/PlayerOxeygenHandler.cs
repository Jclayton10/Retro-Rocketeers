using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOxeygenHandler : MonoBehaviour
{
    public float playersCurrentOxygenLevel;
    private float playersTotalOxygenLevel;
    public float oxygenRegenRate = 5.0f;
    public float oxygenDepleteRate = 5.0f;
    private bool isPlayerInside = false; // Flag to track if the player is inside the oxygen sphere

  
    private void Start()
    {
        playersTotalOxygenLevel = playersCurrentOxygenLevel;
    }

    private void Update()
    {
        // Check if the player is inside the oxygen sphere before regenerating oxygen
        if (isPlayerInside && playersCurrentOxygenLevel < playersTotalOxygenLevel)
        {
            playersCurrentOxygenLevel += oxygenRegenRate * Time.deltaTime;

            // Ensure that the oxygen level doesn't exceed the total oxygen level
            playersCurrentOxygenLevel = Mathf.Min(playersCurrentOxygenLevel, playersTotalOxygenLevel);
        }

        if (!isPlayerInside)
        {
            playersCurrentOxygenLevel -= oxygenDepleteRate * Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("OxyegenSystem"))
        {
            isPlayerInside = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("OxyegenSystem"))
        {
            isPlayerInside = false;
        }
    }
}
