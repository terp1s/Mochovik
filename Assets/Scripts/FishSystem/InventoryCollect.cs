using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InventoryCollect : MonoBehaviour
{
    public void Collect(InventoryItem inventoryItem)
    {
        Inventory.Instance.AddItem(inventoryItem);
        Destroy(gameObject);
    }

}
