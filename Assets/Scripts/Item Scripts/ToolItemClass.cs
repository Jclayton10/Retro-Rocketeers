using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Tool Item", menuName = "Item/Tool")]
public class ToolItemClass : ItemClass
{
    [Header("ToolItem")] //data specific to every tool item
    public ToolType toolType;
    public float attackDamage;
    public float defenseBonus;

    public enum ToolType
    {
        weapon,
        pickaxe,
        shield
    }

    public override ToolItemClass GetToolItem() { return this; }
 
}