using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryMgmt : MonoBehaviour
{
    //Singleton 
    public static InventoryMgmt inventoryMgmt;

    //Bool to see if inventory Management is toggled
    public bool on;

    [SerializeField] private GameObject CraftingInventoryBackground;

    private void Start()
    {
        inventoryMgmt = this;

    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (on)
            {
                CloseInventory();
            }

            else
            {
                OpenInventory();
            }
        }
    }

    void OpenInventory()
    {
        on = true;
        CraftingInventoryBackground.SetActive(true);
    }

    void CloseInventory()
    {
        on = false;
        CraftingInventoryBackground.SetActive(false);
    }
}
