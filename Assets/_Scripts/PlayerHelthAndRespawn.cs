using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHelthAndRespawn : MonoBehaviour
{
    public int currentHealth = 100;

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
       
    }

    public void TakeDamge(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Method to handle player death (you can implement your own logic)
    private void Die()
    {
      
    }
}
