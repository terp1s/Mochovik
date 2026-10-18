using UnityEngine;
using UnityEngine.EventSystems;

public class CollectOnClick : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] InventoryItem inventoryItem;
    public INiche CurrentNiche;

    private void Awake()
    {
        CurrentNiche = GetComponentInParent<INiche>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.dragging) return;

        if(CurrentNiche is not null)
        {
            CurrentNiche.OnObjectRemoved();
            CurrentNiche = null;
        }

        Inventory.Instance.AddItem(inventoryItem, this.gameObject);
        Destroy(gameObject);
    }
}
