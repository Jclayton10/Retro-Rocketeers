using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public InventoryManagement inventory;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            //Use the item
            if(inventory.selectedItem != null)
            {
                inventory.selectedItem.Use(this);
            }
        
        }    
    }
}
