using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour//, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    public ItemClass currItem;
    //public int quantity;

    public Image itemImage;
    public RectTransform itemTransform;

    private CanvasGroup cg;
    public Canvas canvas;

    void Start()
    {
        cg = GetComponent<CanvasGroup>();
        UpdateSlotData();
    }

    public void UpdateSlotData()
    {
        if (currItem != null)
        {
            itemImage.enabled = true;
            itemImage.sprite = currItem.itemIcon;
        }
        else
        {
            itemImage.enabled = false;
        }

        itemTransform.anchoredPosition = Vector3.zero;
    }
    /*
    public void OnPointerDown(PointerEventData eventData)
    {
        cg.blocksRaycasts = false;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        bool foundSlot = false;

        foreach (GameObject overObj in eventData.hovered)
        {
            if (overObj != gameObject)
            {
                if (overObj.GetComponent<ItemSlot>())
                {
                    ItemSlot itemSlot = overObj.GetComponent<ItemSlot>();

                    ItemClass prevItem = currItem;

                    currItem = itemSlot.currItem;
                    itemSlot.currItem = prevItem;

                    itemSlot.itemTransform.anchoredPosition = Vector3.zero;
                    itemSlot.UpdateSlotData();
                    UpdateSlotData();

                    foundSlot = true;

                }
            }
        }

        if (!foundSlot)
        {
            itemTransform.anchoredPosition = Vector3.zero;
        }

        cg.blocksRaycasts = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (currItem != null)
        {
            itemTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        }
    }
    */
}