using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;


public class Inventory : MonoBehaviour, IDropHandler
{
    public static Inventory Instance;

    public Transform inventoryUI;
    public GameObject slotPrefab;
    private List<InventorySlot> slots = new();

    private void Awake()
    {
        Instance = this;
    }

    public void AddItem(InventoryItem item)
    {
        GameObject slot = Instantiate(slotPrefab, inventoryUI);
        slots.Add(slot.GetComponent<InventorySlot>());

        slot.GetComponent<InventorySlot>()
            .Setup(item, this);
    }


    public void RemoveItem(InventorySlot slot)
    {
        slots.Remove(slot);
        Destroy(slot.gameObject);
    }

    public void OnDrop(PointerEventData eventData)
    {
        if(eventData.pointerDrag.TryGetComponent<InventoryItem>(out InventoryItem item))
        {
            AddItem(item);
            Destroy(eventData.pointerDrag);
        }
    }
}



