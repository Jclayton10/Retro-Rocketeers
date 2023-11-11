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
        playerObj = GameObject.Find("PlayerObj").transform;
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

        if(playerInDistance && GameMaster.Instance.InteractPressed)
        {
            StorageHandler.storageHandler.itemsInStorage = items;
            StorageHandler.storageHandler.ToggleUI();
        }
    }

    public bool hasSpace(SlotClass inputItem)
    {
        for(int i = 0; i < items.Length; i++)
        {
            Debug.Log(items[i]);
            if (items[i] == null)
                return true;

            if (items[i].GetItem() == inputItem.GetItem() && items[i].GetQuantity() < 999)
            {
                return true;
            }
        }

        return false;
    }

    public void AddItem(SlotClass inputItem)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == null)
                continue;

            if (items[i].GetItem() == inputItem.GetItem() && items[i].GetQuantity() < 999)
            {
                items[i].AddQuantity(1);
                return;
            }
        }
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == null)
            {
                items[i] = new SlotClass(inputItem.GetItem(), 1);
                return;
            }
        }
    }
}
