using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageItem : MonoBehaviour
{
    [SerializeField] SlotClass[] startingItems;
    [SerializeField] SlotClass[] items;
    [SerializeField] GameObject buttonPopup;
    [SerializeField] Transform playerObj;
    [SerializeField] float interactionDistance;

    [SerializeField] bool playerInDistance;

    private void Start()
    {
        items = new SlotClass[items.Length];
        for(int i = 0; i < startingItems.Length; i++)
        {
            items[i] = startingItems[i];
        }
    }

    private void Update()
    {
        playerInDistance = false;
        Debug.Log(Vector3.Distance(playerObj.position, transform.position));
        if (Vector3.Distance(playerObj.position, transform.position) < interactionDistance)
        {
            playerInDistance = true;
            buttonPopup.SetActive(true);
        }
        else
            buttonPopup.SetActive(false);

        if(playerInDistance && Input.GetKeyDown(GameMaster.Instance.interact))
        {
            StorageHandler.storageHandler.itemsInStorage = items;
            StorageHandler.storageHandler.ToggleUI();
        }
    }

    public bool hasSpace(SlotClass inputItem)
    {
        foreach(SlotClass item in items)
        {
            if (item.GetItem() == null)
                return true;
            if(item.GetItem() == inputItem.GetItem() && item.GetQuantity() < 999) {
                return true;
            }
        }
        return false;
    }

    public void AddItem(SlotClass inputItem)
    {
        foreach (SlotClass item in items)
        {
            if (item.GetItem() == inputItem.GetItem() && item.GetQuantity() < 999)
            {
                item.AddQuantity(1);
                return;
            }
        }
        foreach (SlotClass item in items)
        {
            if (item.GetItem() == null)
            {
                item.AddItem(inputItem.GetItem(), 1);
                return;
            }
        }
    }
}
