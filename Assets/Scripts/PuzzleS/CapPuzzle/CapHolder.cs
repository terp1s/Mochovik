using UnityEngine;
using UnityEngine.AdaptivePerformance;
using UnityEngine.EventSystems;

public class CapHolder : MonoBehaviour, IDropHandler, IDragHandler, IBeginDragHandler, IPointerClickHandler
{
    public CapPuzzle manager;
    public CapPuzzlePiece cap;
    public bool IsOccupied => cap is not null;


    private void Awake()
    {
        manager = FindAnyObjectByType<CapPuzzle>();
        cap = GetComponentInChildren<CapPuzzlePiece>();
    }
    public void RemoveCap()
    {
        if(cap != null)
        {
            cap = null;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (IsOccupied)
        {
            // Transfer ownership of the drag to the cap piece
            eventData.pointerDrag = cap.gameObject;
            ExecuteEvents.Execute(cap.gameObject, eventData, ExecuteEvents.beginDragHandler);
        }
        else
        {
            // Nothing to drag on an empty rope
            eventData.pointerDrag = null;
        }
    }

    // Required by EventSystem to recognise this object as draggable on pointer down
    public void OnDrag(PointerEventData eventData)
    {
        // Unity will automatically route OnDrag to eventData.pointerDrag (the cap)
    }


    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("Dropped  on " + gameObject.name);
        GameObject drag = eventData.pointerDrag;

        if (drag is null) { return; }
        if (!drag.TryGetComponent<CapPuzzlePiece>(out CapPuzzlePiece capPuzzlePiece)) { return; }

        if (!IsOccupied)
        {
            cap = capPuzzlePiece;
            capPuzzlePiece.AttachTo(this);
            manager.CheckPuzzleCompletion();
        }

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (IsOccupied)
        {
            // Transfer ownership of the click to the cap piece
            eventData.pointerClick = cap.gameObject;
            ExecuteEvents.Execute(cap.gameObject, eventData, ExecuteEvents.pointerClickHandler);
        }
        else
        {
            // Nothing to click on an empty rope
            eventData.pointerDrag = null;
        }
    }
}
