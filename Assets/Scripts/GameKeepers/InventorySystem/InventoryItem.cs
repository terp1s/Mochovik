using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] protected ItemDetailPanel descriptionPanel;
    public Sprite Sprite;
    public InventorySlot Slot { get; set; }
    public GameObject UiPrefab;
    public RectTransform UiRectTransform { get; private set; }
    public GameObject WorldPrefab;

    protected CanvasGroup canvasGroup;
    protected Transform originalParent;


    [HideInInspector] public bool wasPlaced = false;


    protected void Awake()
    {
        Slot = GetComponentInParent<InventorySlot>();
        GetComponent<Image>().sprite = Sprite;
        GetComponent<Image>().preserveAspect = true;
        UiRectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        descriptionPanel = FindAnyObjectByType<ItemDetailPanel>(FindObjectsInactive.Include);
    }

    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        wasPlaced = false;
        originalParent = transform.parent;
        canvasGroup.blocksRaycasts = false;
        transform.SetParent(GetComponentInParent<Canvas>().transform, true);

        /*
        if (Slot is not null)
        {
            Inventory.Instance.RemoveItem(Slot);
            Slot = null;
        }
        */
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
        UiRectTransform.anchoredPosition += eventData.delta;
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;

        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(eventData.position);

        Debug.Log($"Mouse screen: {eventData.position}");
        Debug.Log($"Mouse world: {mousePosition}");

        RaycastHit2D[] hits = Physics2D.RaycastAll(
            mousePosition,
            Vector2.zero
        );

        foreach (RaycastHit2D hit in hits)
        {
            IItemReceiver receiver =
                hit.collider.GetComponentInParent<IItemReceiver>();

            if (receiver == null)
                continue;

            if (!receiver.CanReceiveItem(this))
                continue;

            receiver.OnReceiveItem(this);
            wasPlaced = true;
            break;
        }

        if (wasPlaced)
        {
            // Successfully placed in the world: delete the slot and destroy this item
            if (Slot != null)
            {
                Inventory.Instance.RemoveItem(Slot);
            }
            Destroy(gameObject);
        }
        else
        {
            // Drop failed or cancelled: Snap right back into the existing slot!
            SnapBackToSlot();
        }
    }

    private void SnapBackToSlot()
    {
        if (Slot != null)
        {
            transform.SetParent(Slot.transform, false);
            UiRectTransform.anchoredPosition = Vector2.zero;
        }
    }

    public virtual GameObject SpawnInWorld(Vector3 position, Transform parent)
    {
        wasPlaced = true;
        GameObject spawnedObject = Instantiate(WorldPrefab, parent);
        spawnedObject.transform.localPosition = position;

        return spawnedObject;
    }
}