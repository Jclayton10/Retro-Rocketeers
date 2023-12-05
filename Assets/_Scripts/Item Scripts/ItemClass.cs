using UnityEngine;

public class ItemClass : ScriptableObject
{
    [Header("Item")] //data that every item has
    public string itemName;
    public Sprite itemIcon;
    public GameObject prefab;
    public bool isStackable = true;
    public bool healer = false;
    //[TextArea(15, 20)]
    //public string description;
    //public int quantity;

    public virtual void Use()
    {
        Debug.Log("Used: Item");
    }

    public virtual ItemClass GetItem() { return this; }
    public virtual DefaultItemClass GetDefaultItem() { return null; }
    public virtual ToolItemClass GetToolItem() { return null; }
    public virtual FoodItemClass GetFoodItem() { return null; }
}

