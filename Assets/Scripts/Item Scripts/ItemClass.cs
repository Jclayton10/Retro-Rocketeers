using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemClass : ScriptableObject
{
    [Header("Item")] //data that every item has
    public string itemName;
    public Sprite itemIcon;
   // public GameObject prefab;
    public bool isStackable = true;
    [TextArea(15, 20)]
    public string description;

    public abstract ItemClass GetItem();
    public abstract DefaultItemClass GetDefaultItem();
    public abstract ToolItemClass GetToolItem();
    public abstract FoodItemClass GetFoodItem();
}
