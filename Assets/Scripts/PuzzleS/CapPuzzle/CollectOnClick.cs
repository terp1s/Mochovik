using UnityEngine;
using UnityEngine.EventSystems;

public class CollectOnClick : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] InventoryItem inventoryItem;
    public INiche niche;

    private void Awake()
    {
        niche = GetComponentInParent<INiche>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.dragging) return;

        if(niche is not null)
        {
            niche.OnObjectRemoved();
            niche = null;
        }

        Inventory.Instance.AddItem(inventoryItem);
        Destroy(gameObject);
    }
}
