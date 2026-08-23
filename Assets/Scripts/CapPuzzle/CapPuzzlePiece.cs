using UnityEngine;
using UnityEngine.EventSystems;

public class CapPuzzlePiece : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] public CapHolder ParentHolder;
    public CapHolder ChildHolder;
    private Transform originalParent;
    private Collider2D col; 
    private float zDistanceToCamera;
    private Vector3 offset;
    public Vector2Int capIndex;
    public bool IsPlaced => ParentHolder != null;

    protected void Awake()
    {
        col = GetComponent<Collider2D>();

        ChildHolder = GetComponentInChildren<CapHolder>();
        ParentHolder = GetComponentInParent<CapHolder>();


        //capIndex = new Vector2Int((int)gameObject.name[3], (int)gameObject.name[4]);
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

    private Vector3 GetMouseWorldPos(Vector2 screenPosition)
    {
        Vector3 mousePoint = screenPosition;
        mousePoint.z = zDistanceToCamera;
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Only start dragging if the pointer press is on this game object
        GameObject hitObject = eventData.pointerCurrentRaycast.gameObject;

        // 2. If the hit object is not THIS cap (e.g., it's the child rope), cancel the drag
        if (hitObject != this.gameObject)
        {
            
            Debug.Log("Pointer press is not on this game object " + gameObject.name + ". Dragging canceled.");
            eventData.pointerDrag = null;
            return;
        }

        originalParent = transform.parent;

        Debug.Log("Drag Started on " + gameObject.name);
        zDistanceToCamera = Mathf.Abs(Camera.main.transform.position.z - transform.position.z);

        offset = transform.position - GetMouseWorldPos(eventData.position);


        Detach();

        if (col != null) col.enabled = false;


        if (ParentHolder != null)
        {
            ParentHolder.RemoveCap();
            ParentHolder = null;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("Draging " + gameObject.name);

        transform.position = GetMouseWorldPos(eventData.position) + offset;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("Drag ended on " + gameObject.name);

        if (col != null) col.enabled = true;

        if (!IsPlaced)
        {
            transform.SetParent(originalParent);
            transform.localPosition = Vector3.zero;
        }
    }

  
}