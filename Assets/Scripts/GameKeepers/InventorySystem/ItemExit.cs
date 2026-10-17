using UnityEngine;
using UnityEngine.EventSystems;

public class ItemExit : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] ItemDetailPanel detailPanel;

    private void Awake()
    {
        detailPanel = FindAnyObjectByType<ItemDetailPanel>(FindObjectsInactive.Include);
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        detailPanel.Close();
    }
}
