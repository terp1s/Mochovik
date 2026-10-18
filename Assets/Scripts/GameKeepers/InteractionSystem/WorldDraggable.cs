using Ink.Parsed;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class WorldDraggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public INiche CurrentNiche { get; private set; }


    // Return-to-origin data
    protected Transform originalParent;
    protected Vector3 originalPosition;
    protected INiche originalNiche;


    protected Vector3 offset;
    protected float zDistanceToCamera;
    private Collider2D[] allColliders;

    protected SpriteRenderer spriteRenderer;
    private int originalSortingOrder;

    [SerializeField] private int dragSortingOrder = 1000;


    protected virtual void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
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

        originalPosition = transform.position;
        originalParent = transform.parent;
        originalNiche = CurrentNiche;


        if (CurrentNiche != null)
        {
            CurrentNiche.OnObjectHeld();
        }

        transform.SetParent(null);


        zDistanceToCamera = Mathf.Abs(Camera.main.transform.position.z - transform.position.z);
        offset = transform.position - CalculateVisualCenter();

        SetCollidersEnabled(false);

        if (spriteRenderer != null)
        {
            originalSortingOrder = spriteRenderer.sortingOrder;
            spriteRenderer.sortingOrder = dragSortingOrder;
        }


        OnDragStart();
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = GetMouseWorldPos(eventData.position) + offset;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.sortingOrder = originalSortingOrder;
        }

        bool wasPlaced = false;
        IObjectReceiver receiver = FindValidReceiver(eventData);

        if (receiver != null && receiver.CanReceiveObject(this))
        {
            receiver.OnReceiveObject(this);
            wasPlaced = true;

            
            if (originalNiche != null)
            {
                originalNiche.OnObjectRemoved();
            }

            if (receiver is INiche socket)
            {
                CurrentNiche = socket;
            }
            else
            {
                CurrentNiche = null;
            }
        }

        if (!wasPlaced)
        {
            
            transform.SetParent(originalParent);
            transform.position = originalPosition;
            CurrentNiche = originalNiche;

            if (originalNiche != null)
            {
                originalNiche.OnObjectReturned();
            }
        }

        SetCollidersEnabled(true);

        OnDragEnd();
    }

    private Vector3 CalculateVisualCenter()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null && sr.sprite != null)
        {
            return sr.bounds.center;
        }

        return transform.position;
    }

    private IObjectReceiver FindValidReceiver(PointerEventData eventData)
    {

        if (eventData.hovered == null) return null;

        foreach (GameObject hovered in eventData.hovered)
        {
            if (hovered == null) continue;

            // Skip ourselves and any of our children
            if (hovered.transform.IsChildOf(this.transform)) continue;

            IObjectReceiver receiver = hovered.GetComponentInParent<IObjectReceiver>();

            // Ensure the receiver found is not on us or our children
            if (receiver != null && !((Component)receiver).transform.IsChildOf(this.transform) && receiver.CanReceiveObject(this))
            {
                return receiver;
            }
        }

        return null;
    }

    private void SetCollidersEnabled(bool isEnabled)
    {
        allColliders = GetComponentsInChildren<Collider2D>(true);
        for (int i = 0; i < allColliders.Length; i++)
        {
            if (allColliders[i] != null)
                allColliders[i].enabled = isEnabled;
        }
    }


    protected Vector3 GetMouseWorldPos(Vector2 screenPosition)
    {
        Vector3 mousePoint = screenPosition;
        mousePoint.z = zDistanceToCamera;
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }
    public void SetNiche(INiche niche)
    {
        CurrentNiche = niche;
    }

    public void RemoveFromNiche()
    {
        if (CurrentNiche == null)
            return;

        CurrentNiche.OnObjectRemoved();
        CurrentNiche = null;
    }
}