using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public InventoryManagement inventory;

    public static PlayerController playerController { get; private set; }

    [Header("Player Varibles")]
    public int healthRestored;
    public int currentHealth = 100;
    private int maxHealth;

    [Header("Player HealthBar Varibles")]
    public Slider playerHealthSlider;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (inventory.selectedItem != null)
            {
                inventory.selectedItem.Use();

                //Heal();
            }
        }
    }


    public void SetHealthUI(int health)
    {
        playerHealthSlider.value = health;
    }

    //Healing the Player using Food
    //public void Heal(int healthRestored)
    //{
    //    currentHealth += healthRestored;
    //    SetHealthUI(currentHealth);
    //}
}