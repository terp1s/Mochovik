/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour, IPointerClickHandler
{
    public Image icon;
    private ItemData item;


    public void Setup(ItemData newItem, Inventory inv)
    {
        item = newItem;

        icon.sprite = item.icon;
        icon.preserveAspect = true;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Click();
        }
    }

    public void Click()
    {
        Canvas canvas = GetComponentInParent<Canvas>();
        GameObject dragObj = Instantiate(item.uiDragPrefab, canvas.transform);

        var dragInstance = dragObj.AddComponent<UIDragInstance>();
        dragInstance.Setup(item, canvas);

        Inventory.Instance.RemoveItem(item);
    }
}
*/
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlotUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    public Image icon;
    public ItemData item;
    private GameObject currentDragGhost;
    private Canvas canvas;
    public ISlotItemInteractionHandlerer slotItemInteractionHandlerer;

    public void Setup(ItemData newItem)
    {
        item = newItem;
        icon.sprite = item.icon;
        icon.preserveAspect = true;
        canvas = GetComponentInParent<Canvas>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (item.interactionType == ItemInteraction.Click && !eventData.dragging)
        {
            slotItemInteractionHandlerer.HandleSlotInteraction(ItemInteraction.Click);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (item.interactionType != ItemInteraction.Drag)
        {
            eventData.pointerDrag = null; //cancel the drag
            return;
        }

        currentDragGhost = Instantiate(item.uiDragPrefab, canvas.transform);

        var group = currentDragGhost.GetComponent<CanvasGroup>();
        if (group == null) group = currentDragGhost.AddComponent<CanvasGroup>();
        group.blocksRaycasts = false;

        icon.color = new Color(1, 1, 1, 0.5f);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (currentDragGhost != null)
        {
            RectTransform rect = currentDragGhost.GetComponent<RectTransform>();
            Vector2 position;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvas.transform as RectTransform,
                eventData.position,
                canvas.worldCamera,
                out position);

            rect.anchoredPosition = position;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        slotItemInteractionHandlerer.HandleSlotInteraction(ItemInteraction.Drag);
        Destroy(currentDragGhost);
    }

}
