using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public InventoryManagement inventory;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (inventory.selectedItem != null)
            {
                inventory.selectedItem.Use(this);
            }
        }
    }
}