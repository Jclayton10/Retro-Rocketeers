using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Recipe", menuName = "Crafting/CraftingUIRecipe")]
public class CraftingRecipe : ScriptableObject
{
    public ItemClass[] topRow = new ItemClass[3];
    public ItemClass[] midRow = new ItemClass[3];
    public ItemClass[] bottomRow = new ItemClass[3];

    public ItemClass output;

}
