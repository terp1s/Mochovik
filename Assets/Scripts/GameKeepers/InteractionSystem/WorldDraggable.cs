using UnityEngine;
using UnityEngine.EventSystems;

public abstract class WorldDraggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    protected Vector3 offset;
    protected float zDistanceToCamera;
    protected Collider2D col;

    protected virtual void Awake()
    {
        col = GetComponent<Collider2D>();
    }

    protected abstract bool CanStartDrag(PointerEventData eventData);
    protected virtual void OnDragStart() { }
    protected virtual void OnDragEnd() { }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!CanStartDrag(eventData))
        {
            eventData.pointerDrag = null; 
            return;
        }

        zDistanceToCamera = Mathf.Abs(Camera.main.transform.position.z - transform.position.z);
        offset = transform.position - GetMouseWorldPos(eventData.position);

        if (col != null) col.enabled = false;

        OnDragStart();
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = GetMouseWorldPos(eventData.position) + offset;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (col != null) col.enabled = true;
        OnDragEnd();
    }

    protected Vector3 GetMouseWorldPos(Vector2 screenPosition)
    {
        Vector3 mousePoint = screenPosition;
        mousePoint.z = zDistanceToCamera;
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }
}