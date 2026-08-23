using UnityEngine;
using UnityEngine.EventSystems;

public class CapHolder : MonoBehaviour, IDropHandler
{
    public CapPuzzleManager manager;
    public CapPuzzlePiece cap;
    public bool IsOccupied => cap is not null;

    private void Awake()
    {
        manager = FindAnyObjectByType<CapPuzzleManager>();
        cap = GetComponentInChildren<CapPuzzlePiece>();
    }
    public bool TryAssignCap(CapPuzzlePiece newCap)
    {
        if(IsOccupied) { return false; }

        cap = newCap;
        return true;
    }

    public void RemoveCap()
    {
        if(cap != null)
        {
            cap = null;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("Dropped  on " + gameObject.name);

        manager.TryPlaceActiveDrag(this, eventData.pointerDrag);
    }
}
