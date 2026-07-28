using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;

    public Transform inventoryUI;
    public GameObject slotPrefab;
    private List<ItemData> items = new();

    private void Awake()
    {
        Instance = this;
    }

    public void AddItem(ItemData item)
    {
        items.Add(item);

        GameObject slot = Instantiate(slotPrefab, inventoryUI);

        slot.GetComponent<InventorySlotUI>()
            .Setup(item);
    }


    public void RemoveItem(ItemData item)
    {
        int index = items.IndexOf(item);

        if (index == -1)
            return;

        items.RemoveAt(index);

        Destroy(inventoryUI.GetChild(index).gameObject);
    }
}

