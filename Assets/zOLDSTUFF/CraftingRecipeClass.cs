using UnityEngine;

[CreateAssetMenu(fileName = "newCraftingRecipe", menuName = "Crafting/Recipe")]
public class CraftingRecipeClass : ScriptableObject
{
    [Header("Crafting Recipe")]
    public SlotClass[] inputItems;
    public SlotClass outputItem;

    public bool CanCraft(InventoryManagement inventory)
    {
        //Check if we have space in inventory to craft
        if (inventory.isFull())
        {
            return false;
        }

        for (int i = 0; i < inputItems.Length; i++)
        {
            if (inventory.Contains(inputItems[i].GetItem(), inputItems[i].GetQuantity()) == null)
            {
                return false;
            }
        }

        //Return if Inventory has Input Items
        return true;
    }

    public void Craft(InventoryManagement inventory)
    {
        //Remove the Input Items from Inventory
        for (int i = 0; i < inputItems.Length; i++)
        {
            inventory.Remove(inputItems[i].GetItem(), inputItems[i].GetQuantity());
        }

        //Add the Output Item to the Inventory
        inventory.Add(outputItem.GetItem(), outputItem.GetQuantity());
    }
}