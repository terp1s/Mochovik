using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class FishItem : InventoryItem
{
    public SardinePuzzlePiece puzzlePiece;

    protected new void Awake()
    {
        base.Awake();
        puzzlePiece = GetComponent<SardinePuzzlePiece>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        SardinePuzzleManager manager = FindAnyObjectByType<SardinePuzzleManager>();

        if (puzzlePiece.isPlaced)
        {
            manager.RemovePiece(puzzlePiece);
            puzzlePiece.isPlaced = false;
        }

        GetComponent<RectTransform>().sizeDelta = puzzlePiece.Size;
        base.OnBeginDrag(eventData);
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);

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
