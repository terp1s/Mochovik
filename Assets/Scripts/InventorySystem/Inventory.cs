using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Inventory : MonoBehaviour
{
    public static Inventory Instance;

    public Transform inventoryUI;
    public GameObject slotPrefab;
    private List<GameObject> items = new();

    private void Awake()
    {
        Instance = this;
    }

    public void AddItem(InventoryItem item)
    {
        GameObject slot = Instantiate(slotPrefab, inventoryUI);
        items.Add(slot);

        slot.GetComponent<InventorySlot>()
            .Setup(item, this);
    }


    public void RemoveItem(ItemData item)
    {

    }
}


