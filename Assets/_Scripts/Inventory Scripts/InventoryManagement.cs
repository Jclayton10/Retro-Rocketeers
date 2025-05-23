using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManagement : MonoBehaviour
{
    GameMaster GM;

    //Singleton
    public static InventoryManagement inventoryManagement;

    //Bool to see if inventory management is toggled
    public bool on;

    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private GameObject craftingPanel;
    [SerializeField] private GameObject CraftingRecipeCheatSheetPanel;

    [SerializeField] private GameObject itemCursor;

    [SerializeField] private GameObject slotHolder;
    [SerializeField] private GameObject hotbarSlotHolder;
    [SerializeField] private ItemClass itemToAdd;
    [SerializeField] private ItemClass itemToRemove;

    [SerializeField] private SlotClass[] startingItems;

    public SlotClass[] items;

    private GameObject[] slots;
    private GameObject[] hotbarSlots;
    [SerializeField] private GameObject[] craftingSlots;

    [SerializeField] private SlotClass movingSlot;
    [SerializeField] private SlotClass tempSlot;
    [SerializeField] private SlotClass originalSlot;
    bool isMovingItem;

    [SerializeField] private GameObject hotbarSelector;
    [SerializeField] private int selectedSlotIndex = 0;
    public ItemClass selectedItem;

    [Header("Audio")]
    public List<AudioClip> BagRussles;
    public AudioClip BagOpen;
    public AudioClip BagClose;
    public AudioSource Bag;

    private void Start()
    {
        inventoryManagement = this;

        slots = new GameObject[slotHolder.transform.childCount];
        items = new SlotClass[slots.Length + craftingSlots.Length];

        hotbarSlots = new GameObject[hotbarSlotHolder.transform.childCount];
        for (int i = 0; i < hotbarSlots.Length; i++)
        {
            hotbarSlots[i] = hotbarSlotHolder.transform.GetChild(i).gameObject;
        }

        for (int i = 0; i < items.Length; i++)
        {
            items[i] = new SlotClass();
        }

        for (int i = 0; i < startingItems.Length; i++)
        {
            items[i] = startingItems[i];
        }

        //set all the slots
        for (int i = 0; i < slotHolder.transform.childCount; i++)
        {
            slots[i] = slotHolder.transform.GetChild(i).gameObject;
        }


        Add(itemToAdd, 1);
        Remove(itemToRemove);
        RefreshUI();

        GameObject gm = GameObject.Find("Game Master");
        GM = gm.GetComponent<GameMaster>();
        Bag.volume = GM.AudioMaster * GM.AudioSFX;
    }

    private void Update()
    {
        if (GameMaster.Instance.InvPressed)
        {

            if (on)
            {
                CloseInventory();
            }
            else
            {
                OpenInventory();
                RefreshUI();
            }
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            if (on)
            {
                CraftingRecipeCheatSheetPanel.SetActive(false);
            }

            else
            {
                CraftingRecipeCheatSheetPanel.SetActive(true);
            }
        }

        itemCursor.SetActive(isMovingItem);
        itemCursor.transform.position = Input.mousePosition;
        if (isMovingItem)
        {
            itemCursor.GetComponent<Image>().sprite = movingSlot.GetItem().itemIcon;
        }

        if (Input.GetMouseButtonDown(0) && on) //left click
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

        else if (Input.GetMouseButtonDown(1) && on) //right click
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

        if (Input.GetAxis("Mouse ScrollWheel") > 0) //Scrolling Up
        {
            selectedSlotIndex = Mathf.Clamp(selectedSlotIndex + 1, 0, hotbarSlots.Length - 1);
        }

        else if (Input.GetAxis("Mouse ScrollWheel") < 0) //Scrolling Down
        {
            selectedSlotIndex = Mathf.Clamp(selectedSlotIndex - 1, 0, hotbarSlots.Length - 1);
        }

        hotbarSelector.transform.position = hotbarSlots[selectedSlotIndex].transform.position;
        selectedItem = items[selectedSlotIndex + (hotbarSlots.Length * 3)].GetItem();
    }

    void OpenInventory()
    {
        on = true;
        inventoryPanel.SetActive(true);
        craftingPanel.SetActive(true);
        CraftingRecipeCheatSheetPanel.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        Bag.clip = BagOpen;
        Bag.Play();
        RefreshUI();
    }

    void CloseInventory()
    {
        on = false;
        inventoryPanel.SetActive(false);
        craftingPanel.SetActive(false);
        CraftingRecipeCheatSheetPanel.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Bag.clip = BagClose;
        Bag.Play();
    }

    private void Craft(CraftingRecipeClass recipe)
    {
        if (recipe.CanCraft(this))
        {
            recipe.Craft(this);
        }

        else
        {
            Debug.Log("Cannot Craft that Item!");
        }
    }

    #region Inventory Utilities
    public void RefreshUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            try
            {
                slots[i].transform.GetChild(0).GetComponent<Image>().enabled = true;
                slots[i].transform.GetChild(0).GetComponent<Image>().sprite = items[i].GetItem().itemIcon;
                if (items[i].GetItem().isStackable)
                {
                    slots[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = items[i].GetQuantity() + "";
                }
                else
                {
                    slots[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
                }
            }

            catch
            {
                slots[i].transform.GetChild(0).GetComponent<Image>().sprite = null;
                slots[i].transform.GetChild(0).GetComponent<Image>().enabled = false;
                slots[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
            }
        }

        RefreshHotbar();
    }

    public void RefreshHotbar()
    {
        for (int i = 0; i < hotbarSlots.Length; i++)
        {
            try
            {
                hotbarSlots[i].transform.GetChild(0).GetComponent<Image>().enabled = true;
                hotbarSlots[i].transform.GetChild(0).GetComponent<Image>().sprite = items[i + (hotbarSlots.Length * 3)].GetItem().itemIcon;

                if (items[i + (hotbarSlots.Length * 3)].GetItem().isStackable)
                {
                    hotbarSlots[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = items[i + (hotbarSlots.Length * 3)].GetQuantity() + "";
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

        RefreshCraftingMenu();
    }

    public void RefreshCraftingMenu()
    {
        for (int i = 0; i < craftingSlots.Length; i++)
        {
            try
            {
                craftingSlots[i].transform.GetChild(0).GetComponent<Image>().enabled = true;
                craftingSlots[i].transform.GetChild(0).GetComponent<Image>().sprite = items[i + slots.Length].GetItem().itemIcon;

                if (items[i + slots.Length].GetItem().isStackable)
                {
                    craftingSlots[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = items[i + slots.Length].GetQuantity() + "";
                }
                else
                {
                    craftingSlots[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
                }
            }

            catch
            {
                craftingSlots[i].transform.GetChild(0).GetComponent<Image>().sprite = null;
                craftingSlots[i].transform.GetChild(0).GetComponent<Image>().enabled = false;
                craftingSlots[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
            }
        }
    }

    public bool Add(ItemClass item, int quantity)
    {
        //check if inventory contains items

        SlotClass slot = Contains(item);
        if (slot != null && slot.GetItem().isStackable)
        {
            slot.AddQuantity(quantity);
        }
        else
        {
            for (int i = 0; i < items.Length; i++)
            {
                if (items[i].GetItem() == null) //this is an empty slot
                {
                    items[i].AddItem(item, quantity);
                    break;
                }
            }
        }
        RefreshUI();
        return true;
    }

    public bool Remove(ItemClass item)
    {
        //items.Remove(item);
        SlotClass temp = Contains(item);
        if (temp != null)
        {
            if (temp.GetQuantity() > 1)
            {
                temp.SubQuantity(1);
            }
            else
            {
                int slotToRemoveIndex = 0;

                for (int i = 0; i < items.Length; i++)
                {
                    if (items[i].GetItem() == item)
                    {
                        slotToRemoveIndex = i;
                        break;
                    }
                }

                items[slotToRemoveIndex].Clear();
            }
        }
        else
        {
            return false;
        }

        RefreshUI();
        return true;

    }

    public bool Remove(ItemClass item, int quantity)
    {
        //items.Remove(item);
        SlotClass temp = Contains(item);
        if (temp != null)
        {
            if (temp.GetQuantity() > 1)
            {
                temp.SubQuantity(quantity);
            }
            else
            {
                int slotToRemoveIndex = 0;

                for (int i = 0; i < items.Length; i++)
                {
                    if (items[i].GetItem() == item)
                    {
                        slotToRemoveIndex = i;
                        break;
                    }
                }

                items[slotToRemoveIndex].Clear();
            }
        }
        else
        {
            return false;
        }

        RefreshUI();
        return true;

    }

    public void UseSelected()
    {
        items[selectedSlotIndex + (hotbarSlots.Length * 3)].SubQuantity(1);
        RefreshUI();
    }

    public bool isFull()
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].GetItem() == null)
            {
                return false;
            }
        }
        return true;
    }

    public SlotClass Contains(ItemClass item)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].GetItem() == item)
            {
                return items[i];
            }
        }
        return null;
    }

    public SlotClass Contains(ItemClass item, int quantity)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].GetItem() == item && items[i].GetQuantity() >= quantity)
            {
                return items[i];
            }
        }
        return null;
    }

    #endregion

    #region Moving Inventory Items
    private bool BeginItemMove()
    {
        originalSlot = GetClosestSlot();
        if (originalSlot == null || originalSlot.GetItem() == null)
        {
            return false;
        }

        movingSlot = new SlotClass(originalSlot);
        originalSlot.Clear();
        isMovingItem = true;
        RefreshUI();

        Bag.clip = BagRussles[Random.Range(0, BagRussles.Count)];
        Bag.Play();


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
        originalSlot = GetClosestSlot();
        if (originalSlot == null)
        {
            Add(movingSlot.GetItem(), movingSlot.GetQuantity());
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
            }
        }
        Bag.clip = BagRussles[Random.Range(0, BagRussles.Count)];
        Bag.Play();


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
        return true;

    }
    private SlotClass GetClosestSlot()
    {
        Debug.Log(Input.mousePosition);
        for (int i = 0; i < slots.Length; i++)
        {
            if (Vector2.Distance(slots[i].transform.position, Input.mousePosition) <= 32)
            {
                return items[i];
            }
        }
        for (int i = 0; i < craftingSlots.Length; i++)
        {
            if (Vector2.Distance(craftingSlots[i].transform.position, Input.mousePosition) <= 32)
            {
                return items[i + slots.Length];
            }
        }
        return null;
    }
    #endregion
}