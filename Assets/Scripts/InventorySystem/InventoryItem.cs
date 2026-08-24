using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Sprite Sprite;
    public InventorySlot Slot;
    public GameObject UiPrefab;
    public RectTransform UiRectTransform;
    public GameObject WorldPrefab;

    protected CanvasGroup canvasGroup;
    protected Transform originalParent;

    [HideInInspector] public bool wasPlaced = false;


    protected void Awake()
    {
        Slot = this.GetComponentInParent<InventorySlot>();
        GetComponent<Image>().sprite = Sprite;
        GetComponent<Image>().preserveAspect = true;
        UiRectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        wasPlaced = false;
        originalParent = transform.parent;
        canvasGroup.blocksRaycasts = false;
        transform.SetParent(GetComponentInParent<Canvas>().transform, true);

        if (Slot is not null)
        {
            Inventory.Instance.RemoveItem(Slot);
            Slot = null;
        }
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
        UiRectTransform.anchoredPosition += eventData.delta;
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
    }

    public void SpawnInWorld(Vector3 position, Transform parent)
    {
        wasPlaced = true;
        GameObject spawnedObject = Instantiate(WorldPrefab, parent);
        spawnedObject.transform.localPosition = position;
    }
}