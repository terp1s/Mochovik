using UnityEngine;
using UnityEngine.AdaptivePerformance;
using UnityEngine.EventSystems;

public class CapHolder : MonoBehaviour, IObjectReceiver, IDragHandler, IBeginDragHandler, IPointerClickHandler
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
            eventData.pointerDrag = cap.gameObject;
            ExecuteEvents.Execute(cap.gameObject, eventData, ExecuteEvents.beginDragHandler);
        }
        else
        {
            eventData.pointerDrag = null;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
    }

    /*
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
    */
    public void OnPointerClick(PointerEventData eventData)
    {
        if (IsOccupied)
        {
            eventData.pointerClick = cap.gameObject;
            ExecuteEvents.Execute(cap.gameObject, eventData, ExecuteEvents.pointerClickHandler);
        }
        else
        {
            eventData.pointerDrag = null;
        }
    }

    public bool CanReceiveObject(WorldDraggable obj)
    {
        if (!obj.TryGetComponent<CapPuzzlePiece>(out CapPuzzlePiece capPuzzlePiece)) { return false; }

        if (IsOccupied) { return false; }

        return true;
    }

    public void OnReceiveObject(WorldDraggable obj)
    {
        obj.TryGetComponent<CapPuzzlePiece>(out CapPuzzlePiece capPuzzlePiece);
        cap = capPuzzlePiece;
        capPuzzlePiece.AttachTo(this);
        manager.CheckPuzzleCompletion();
    }
}
