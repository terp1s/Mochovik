using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{
    public Sprite Sprite;
    public InventorySlot Slot;
    public ItemInteraction interaction;
    public GameObject UiPrefab;
    public RectTransform UiRectTransform;

    protected void Awake()
    {
        Slot = this.GetComponentInParent<InventorySlot>();
        GetComponent<Image>().sprite = Sprite;
        GetComponent<Image>().preserveAspect = true;
        UiRectTransform = GetComponent<RectTransform>();
    }
}