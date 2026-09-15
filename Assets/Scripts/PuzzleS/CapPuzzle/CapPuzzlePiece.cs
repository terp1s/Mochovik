using UnityEngine;
using UnityEngine.EventSystems;

public class CapPuzzlePiece : WorldDraggable
{
    [SerializeField] public CapHolder ParentHolder;
    public CapHolder ChildHolder;
    private Transform originalParent;
   
    public Vector2Int capIndex;
    public bool IsPlaced => ParentHolder != null;

    protected new void Awake()
    {
        base.Awake();
        col = GetComponent<Collider2D>();

        ChildHolder = GetComponentInChildren<CapHolder>();
        ParentHolder = GetComponentInParent<CapHolder>();
    }

    public void AttachTo(CapHolder holder)
    {
        ParentHolder = holder;

        transform.SetParent(ParentHolder.transform);
        transform.localPosition = Vector3.zero;
    }

    public void Detach()
    {
        if (ParentHolder != null)
            ParentHolder.RemoveCap();

        ParentHolder = null;
        transform.SetParent(null);
    }

    protected override void OnDragStart()
    {
        originalParent = transform.parent;

        Detach();

        if (ParentHolder != null)
        {
            ParentHolder.RemoveCap();
            ParentHolder = null;
        }
    }

    protected override void OnDragEnd()
    {
        if (!IsPlaced)
        {
            transform.SetParent(originalParent);
            transform.localPosition = Vector3.zero;
        }
    }

    protected override bool CanStartDrag(PointerEventData eventData)
    {
        GameObject hitObject = eventData.pointerCurrentRaycast.gameObject;
        return hitObject == this.gameObject;
    }
}