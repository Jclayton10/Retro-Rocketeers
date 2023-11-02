using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemProducer : MonoBehaviour
{
    [SerializeField] StorageItem itemRef;
    [SerializeField] SlotClass item;

    [SerializeField] float delay;

    private void Start()
    {
        Invoke("AddItem", delay);
    }

    private void AddItem()
    {
        if (itemRef.hasSpace(item))
        {
            itemRef.AddItem(item);
        }
        Invoke("AddItem", delay);
    }
}
