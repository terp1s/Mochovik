using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] protected ItemDetailPanel descriptionPanel;
    public Sprite Sprite;
    public InventorySlot Slot { get; set; }
    public GameObject UiPrefab;
    public RectTransform UiRectTransform { get; private set; }
    public GameObject WorldPrefab;

    protected CanvasGroup canvasGroup;
    protected Transform originalParent;
    protected Canvas rootCanvas;


    [HideInInspector] public bool wasPlaced = false;


    protected void Awake()
    {
        Slot = GetComponentInParent<InventorySlot>();
        GetComponent<Image>().sprite = Sprite;
        GetComponent<Image>().preserveAspect = true;
        UiRectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        rootCanvas = GetComponentInParent<Canvas>();
        descriptionPanel = FindAnyObjectByType<ItemDetailPanel>(FindObjectsInactive.Include);
    }

    public virtual void Setup(GameObject obj)
    {

    }
   
    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        wasPlaced = false;
        originalParent = transform.parent;
        canvasGroup.blocksRaycasts = false;
        transform.SetParent(GetComponentInParent<Canvas>().transform, true);

        if (CursorManager.Instance != null)
        {
            CursorManager.Instance.SetDragging(true);
        }
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
        if (rootCanvas != null)
        {
            UiRectTransform.anchoredPosition += eventData.delta / rootCanvas.scaleFactor;
        }
        else
        {
            UiRectTransform.anchoredPosition += eventData.delta;
        }
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
            if (Slot != null)
            {
                Inventory.Instance.RemoveItem(Slot);
            }
            Destroy(gameObject);
        }
        else
        {
            SnapBackToSlot();
        }

        if (CursorManager.Instance != null)
        {
            CursorManager.Instance.SetDragging(false);
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