using UnityEngine;

public class ItemDetailPanel : MonoBehaviour
{
    [SerializeField] GameObject currentPanel;

    private void Awake()
    {
        //gameObject.SetActive(false);
    }
    public void Show(GameObject panelPrefab)
    {
        Close();

        gameObject.SetActive(true);

        currentPanel = Instantiate(panelPrefab, transform);
        currentPanel.SetActive(true);


        RectTransform rect = currentPanel.GetComponent<RectTransform>();

        rect.localScale = Vector3.one;
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;

    }

    public void Close()
    {
        if (currentPanel is not null)
        {
            Destroy(currentPanel);
            currentPanel = null;
        }

        gameObject.SetActive(false);
    }
}
