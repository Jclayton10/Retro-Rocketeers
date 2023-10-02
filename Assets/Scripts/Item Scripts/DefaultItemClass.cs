using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Default Item", menuName = "Item/Default")]
public class DefaultItemClass : ItemClass
{
    public override void Use(PlayerController caller)
    {
        //base.Use(caller);
    }

    public override DefaultItemClass GetDefaultItem() { return this; }

}
