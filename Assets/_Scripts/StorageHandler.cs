using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StorageHandler : MonoBehaviour
{
    [SerializeField] bool toggled = false;

    public static StorageHandler storageHandler;

    [SerializeField] GameObject hotbar;
    [SerializeField] GameObject selector;

    [SerializeField] GameObject storagePanel;
    [SerializeField] GameObject itemCursor;

    public SlotClass[] itemsInStorage;

    [SerializeField] private GameObject[] inventorySlots;
    [SerializeField] private GameObject[] hotbarSlots;
    [SerializeField] private GameObject[] storageSlots;

    [SerializeField] private SlotClass movingSlot;
    [SerializeField] private SlotClass tempSlot;
    [SerializeField] private SlotClass originalSlot;

    bool isMovingItem;

    private void Start()
    {
        storageHandler = this;

        itemsInStorage = new SlotClass[storageSlots.Length];
    }

    private void Update()
    {
        if(isMovingItem)
            itemCursor.transform.position = Input.mousePosition;

        if (Input.GetMouseButtonDown(0) && toggled) //left click
        {
            //find the closest slot (which would be the slot we clicked on)
            if (isMovingItem)
            {
                //end item move
                EndItemMove();
            }
            else
            {
                BeginItemMove();
            }
        }

        else if (Input.GetMouseButtonDown(1) && toggled) //right click
        {
            if (isMovingItem)
            {
                //end item move
                EndItemMove_Single();
            }
            else
            {
                BeginItemMove_Half();
            }
        }
    }

    public void ToggleUI()
    {
        toggled = !toggled;
        if (toggled)
        {
            RefreshUI();
            storagePanel.SetActive(true);
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            Time.timeScale = 0;
            hotbar.SetActive(false);
            selector.SetActive(false);
        }
        else
        {
            storagePanel.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Time.timeScale = 1;
            hotbar.SetActive(true);
            selector.SetActive(true);
        }

    }

    #region Inventory Utilities
    public void RefreshUI()
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            try
            {
                inventorySlots[i].transform.GetChild(0).GetComponent<Image>().enabled = true;
                inventorySlots[i].transform.GetChild(0).GetComponent<Image>().sprite = InventoryManagement.inventoryManagement.items[i].GetItem().itemIcon;
                if (InventoryManagement.inventoryManagement.items[i].GetItem().isStackable)
                {
                    inventorySlots[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = InventoryManagement.inventoryManagement.items[i].GetQuantity() + "";
                }
                else
                {
                    inventorySlots[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
                }
            }

            catch
            {
                inventorySlots[i].transform.GetChild(0).GetComponent<Image>().sprite = null;
                inventorySlots[i].transform.GetChild(0).GetComponent<Image>().enabled = false;
                inventorySlots[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
            }
        }

        RefreshHotbar();
        RefreshStorage();
    }

    public void RefreshStorage()
    {
        for (int i = 0; i < storageSlots.Length; i++)
        {
            try
            {
                storageSlots[i].transform.GetChild(0).GetComponent<Image>().enabled = true;
                storageSlots[i].transform.GetChild(0).GetComponent<Image>().sprite = itemsInStorage[i].GetItem().itemIcon;
                if (itemsInStorage[i].GetItem().isStackable)
                {
                    storageSlots[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = itemsInStorage[i].GetQuantity() + "";
                }
                else
                {
                    storageSlots[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
                }
            }

            catch
            {
                storageSlots[i].transform.GetChild(0).GetComponent<Image>().sprite = null;
                storageSlots[i].transform.GetChild(0).GetComponent<Image>().enabled = false;
                storageSlots[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
            }
        }
    }

    public void RefreshHotbar()
    {
        for (int i = 0; i < hotbarSlots.Length; i++)
        {
            try
            {
                hotbarSlots[i].transform.GetChild(0).GetComponent<Image>().enabled = true;
                hotbarSlots[i].transform.GetChild(0).GetComponent<Image>().sprite = InventoryManagement.inventoryManagement.items[i + (hotbarSlots.Length * 3)].GetItem().itemIcon;

                if (InventoryManagement.inventoryManagement.items[i + (hotbarSlots.Length * 3)].GetItem().isStackable)
                {
                    hotbarSlots[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = InventoryManagement.inventoryManagement.items[i + (hotbarSlots.Length * 3)].GetQuantity() + "";
                }
                else
                {
                    hotbarSlots[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
                }
            }

            catch
            {
                hotbarSlots[i].transform.GetChild(0).GetComponent<Image>().sprite = null;
                hotbarSlots[i].transform.GetChild(0).GetComponent<Image>().enabled = false;
                hotbarSlots[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
            }
        }
    }

    public bool AddToInventory(ItemClass item, int quantity)
    {
        InventoryManagement.inventoryManagement.Add(item, quantity);
        RefreshUI();
        return true;
    }

    public bool isFull()
    {
        for (int i = 0; i < itemsInStorage.Length; i++)
        {
            if (itemsInStorage[i].GetItem() == null)
            {
                return false;
            }
        }
        return true;
    }

    public SlotClass Contains(ItemClass item)
    {
        for (int i = 0; i < itemsInStorage.Length; i++)
        {
            if (itemsInStorage[i].GetItem() == item)
            {
                return itemsInStorage[i];
            }
        }
        return null;
    }

    public SlotClass Contains(ItemClass item, int quantity)
    {
        for (int i = 0; i < itemsInStorage.Length; i++)
        {
            if (itemsInStorage[i].GetItem() == item && itemsInStorage[i].GetQuantity() >= quantity)
            {
                return itemsInStorage[i];
            }
        }
        return null;
    }

    #endregion

    #region Moving Inventory Items
    private bool BeginItemMove()
    {
        Debug.Log("Starting At: ");
        originalSlot = GetClosestSlot();
        if (originalSlot == null || originalSlot.GetItem() == null)
        {
            return false;
        }

        movingSlot = new SlotClass(originalSlot);
        itemCursor.SetActive(true);
        itemCursor.GetComponent<Image>().sprite = movingSlot.GetItem().itemIcon;
        originalSlot.Clear();
        isMovingItem = true;
        RefreshUI();


        /*
        Bag.clip = BagRussles[Random.Range(0, BagRussles.Count)];
        Bag.Play();
        */

        return true;
    }

    private bool BeginItemMove_Half()
    {
        originalSlot = GetClosestSlot();
        if (originalSlot == null || movingSlot.GetItem() == null)
        {
            return false;
        }

        movingSlot = new SlotClass(originalSlot.GetItem(), Mathf.CeilToInt(originalSlot.GetQuantity() / 2f)); //CeilToInt Rounds Up, FloorToInt Rounds Down
        itemCursor.SetActive(true);
        itemCursor.GetComponent<Image>().sprite = movingSlot.GetItem().itemIcon;
        originalSlot.SubQuantity(Mathf.CeilToInt(originalSlot.GetQuantity() / 2f));
        if (originalSlot.GetQuantity() == 0)
        {
            originalSlot.Clear();
        }
        isMovingItem = true;
        RefreshUI();


        return true;

    }

    private bool EndItemMove()
    {
        isMovingItem = false;
        Debug.Log("Moving To: ");
        originalSlot = GetClosestSlot();

        if (originalSlot == null)
        {
            AddToInventory(movingSlot.GetItem(), movingSlot.GetQuantity());
            movingSlot.Clear();
        }
        else
        {

            if (originalSlot.GetItem() != null)
            {
                if (originalSlot.GetItem() == movingSlot.GetItem())
                {
                    if (originalSlot.GetItem().isStackable)
                    {
                        originalSlot.AddQuantity(movingSlot.GetQuantity());
                        movingSlot.Clear();
                    }
                    else
                        return false;

                }
                else
                {
                    tempSlot = new SlotClass(movingSlot);

                    BeginItemMove();

                    originalSlot.AddItem(tempSlot.GetItem(), tempSlot.GetQuantity());

                    RefreshUI();
                    return true;
                }
            }

            else
            {
                originalSlot.AddItem(movingSlot.GetItem(), movingSlot.GetQuantity());
                movingSlot.Clear();
                RefreshUI();
            }
        }

        /*
        Bag.clip = BagRussles[Random.Range(0, BagRussles.Count)];
        Bag.Play();
        */

        itemCursor.SetActive(false);

        RefreshUI();
        return true;
    }

    private bool EndItemMove_Single()
    {
        originalSlot = GetClosestSlot();
        if (originalSlot == null)
        {
            return false;
        }

        if (originalSlot.GetItem() != null && originalSlot.GetItem() != movingSlot.GetItem())
        {
            return false;
        }

        movingSlot.SubQuantity(1);
        if (originalSlot.GetItem() != null && originalSlot.GetItem() == movingSlot.GetItem())
        {
            originalSlot.AddQuantity(1);
        }
        else
        {
            originalSlot.AddItem(movingSlot.GetItem(), 1);
        }

        if (movingSlot.GetQuantity() < 1)
        {
            isMovingItem = false;
            movingSlot.Clear();
        }
        else
        {
            isMovingItem = true;
        }

        RefreshUI();

        itemCursor.SetActive(false);

        return true;

    }
    private SlotClass GetClosestSlot()
    {
        Debug.Log(Input.mousePosition);
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            if (Vector2.Distance(inventorySlots[i].transform.position, Input.mousePosition) <= 32)
            {
                Debug.Log($"Slot: Inventory: {i}");
                return InventoryManagement.inventoryManagement.items[i];
            }
        }
        for (int i = 0; i < hotbarSlots.Length; i++)
        {
            if (Vector2.Distance(hotbarSlots[i].transform.position, Input.mousePosition) <= 32)
            {
                Debug.Log($"Slot: Hotbar: {i}");
                return InventoryManagement.inventoryManagement.items[i + inventorySlots.Length];
            }
        }
        for (int i = 0; i < storageSlots.Length; i++)
        {
            if (Vector2.Distance(storageSlots[i].transform.position, Input.mousePosition) <= 32)
            {
                Debug.Log($"Slot: Storage: {i}");
                
                return itemsInStorage[i];
                
            }
        }

        return null;
    }
    #endregion
}
