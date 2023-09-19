using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Default Item", menuName = "Item/Default")]
public class DefaultItemClass : ItemClass
{
    public override ItemClass GetItem() { return this; }
    public override ToolItemClass GetToolItem() { return null; }
    public override DefaultItemClass GetDefaultItem() { return this; }
    public override FoodItemClass GetFoodItem() { return null; }
}
