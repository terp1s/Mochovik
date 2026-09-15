using UnityEngine;
using UnityEngine.EventSystems;

public class CollectOnClick : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] InventoryItem inventoryItem;
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.dragging) return;
        Inventory.Instance.AddItem(inventoryItem);
        Destroy(gameObject);
    }
}
