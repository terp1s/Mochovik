using UnityEngine;
using UnityEngine.EventSystems;
using static Unity.Burst.Intrinsics.X86.Avx;

public class CapPuzzlePiece : WorldDraggable
{
    [SerializeField] public CapHolder ParentHolder;
    [SerializeField] public CapHolder ChildHolder;
    private CapHolder originalParentHolder;
   
    public Vector2Int capIndex;
    public bool IsPlaced => ParentHolder != null;

    protected new void Awake()
    {
        base.Awake();

        ParentHolder = GetComponentInParent<CapHolder>();
        ChildHolder = GetComponentInChildren<CapHolder>();
    }

    public void AttachTo(CapHolder holder)
    {
        ParentHolder = holder;

        if (ParentHolder != null)
        {
            ParentHolder.cap = this;

            transform.SetParent(ParentHolder.transform, true);
            transform.localPosition = Vector3.zero;
        }

    }

    public void Detach()
    {
        if (ParentHolder != null)
            ParentHolder.RemoveCap();

        ParentHolder = null;
        transform.SetParent(null);
        transform.localPosition -= new Vector3(0, 0, 0.2f); 
    }

    protected override void OnDragStart()
    {
        originalParent = transform.parent;
        originalParentHolder = ParentHolder;

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
            if (originalParentHolder != null)
            {
                AttachTo(originalParentHolder);
            }
            else if (originalParent != null)
            {
                transform.SetParent(originalParent, true);
                transform.localPosition = Vector3.zero;
            }

         
        }
    }

    protected override bool CanStartDrag(PointerEventData eventData)
    {
        return true;
    }

}

