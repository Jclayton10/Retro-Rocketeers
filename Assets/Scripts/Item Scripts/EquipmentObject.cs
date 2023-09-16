using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Equipment Object", menuName = "Inventory Management System/Items/Equipment")]
public class EquipmentObject : ItemObject
{
    public float attackbonus;
    public float defensebonus;

    public void Awake()
    {
        type = ItemType.Equipment;
    }
}
