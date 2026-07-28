using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    public Transform parentAfterDrag;
    public ItemData item;
    public Image icon;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (item.interactionType == ItemInteraction.Click && !eventData.dragging)
        {
            item.slotInteractionHandlerer.HandleSlotInteraction(ItemInteraction.Click);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (item.interactionType != ItemInteraction.Drag)
        {
            eventData.pointerDrag = null; //cancel the drag
            return;
        }

        icon.raycastTarget = false;
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        item.slotInteractionHandlerer.HandleSlotInteraction(ItemInteraction.Drag);
    }

}

    
