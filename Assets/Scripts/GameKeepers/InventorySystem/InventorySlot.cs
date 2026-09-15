using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public InventoryItem InventoryItem;
    public Inventory inventory;
    public GameObject UiItem;
    public void Setup(InventoryItem item, Inventory inv)
    {
        InventoryItem = item;
        inventory = inv;
        UiItem = Instantiate(item.UiPrefab, transform);
        UiItem.GetComponent<RectTransform>().localPosition = Vector2.zero;
        LayoutElement layout = GetComponent<LayoutElement>();
        UiItem.GetComponent<RectTransform>().sizeDelta = new Vector2(layout.preferredWidth, layout.preferredHeight);
        UiItem.GetComponent<InventoryItem>().Slot = this;
    }
}
