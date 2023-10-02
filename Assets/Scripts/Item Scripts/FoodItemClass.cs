using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Food Item", menuName = "Item/Food")]
public class FoodItemClass : ItemClass
{
    [Header("FoodItem")] //data specific to every food item
    public FoodType foodType;
    public float healthRestored;

    public enum FoodType
    {
        lowquality,
        mediumquality,
        highquality
    }

    public override FoodItemClass GetFoodItem() { return this; }
}
