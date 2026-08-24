using UnityEngine;
using UnityEngine.AdaptivePerformance;
using UnityEngine.EventSystems;

public class CapHolder : MonoBehaviour, IDropHandler
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
}
