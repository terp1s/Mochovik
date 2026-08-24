using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class MasonJarItem : InventoryItem
{
    public bool isFilled = false;
    
    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);

        if (!wasPlaced)
        {
            Inventory.Instance.AddItem(this);
        }

        Destroy(gameObject);

    }
}
