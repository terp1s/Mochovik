using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public enum ItemInteraction { Click, Drag }
public class FishItem : InventoryItem, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public PuzzlePiece puzzlePiece;
    private CanvasGroup canvasGroup;
    private Transform originalParent;

    protected new void Awake()
    {
        base.Awake();
        interaction = ItemInteraction.Drag;
        puzzlePiece = GetComponent<PuzzlePiece>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        SardinePuzzleManager manager = FindObjectOfType<SardinePuzzleManager>();

        if (puzzlePiece.isPlaced)
        {
            manager.RemovePiece(puzzlePiece);
            puzzlePiece.isPlaced = false;
        }

        GetComponent<RectTransform>().sizeDelta = puzzlePiece.Size;
        canvasGroup.blocksRaycasts = false;
        originalParent = transform.parent;
        transform.SetParent(GetComponentInParent<Canvas>().transform, true);
        canvasGroup.blocksRaycasts = false;
        Debug.Log($"{this.gameObject.name} drag from inventory begun");
    }

    public void OnDrag(PointerEventData eventData)
    {
        //Debug.Log($"{this.gameObject.name} is being dragged from inventory");
        UiRectTransform.anchoredPosition += eventData.delta;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;

        Debug.Log($"{this.gameObject.name} was dragged from inventory");

        if (!puzzlePiece.isPlaced)
        {
            if(Slot != null)
            {
                Inventory.Instance.RemoveItem(Slot);
            }
            Inventory.Instance.AddItem(this);
            Destroy(gameObject);
        }
        else
        {
            Inventory.Instance.RemoveItem(Slot);
        }
    }

}
