using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonSelect : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public GameObject SelectionBrackets;
    [SerializeField] bool isDefault = false;
    [SerializeField] bool isSelected;
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }
    private void Start()
    {
        if (isDefault)
        {
            button.Select();
        }
    }
    public void OnSelect(BaseEventData eventData)
    {
        SelectionBrackets.transform.SetParent(transform);
        SelectionBrackets.transform.localPosition = Vector3.zero;

        isSelected = true;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        isSelected = false;
        StartCoroutine(KeepSelectionRoutine());
    }

    private System.Collections.IEnumerator KeepSelectionRoutine()
    {
        yield return null;
        if (EventSystem.current != null && EventSystem.current.currentSelectedGameObject == null)
        {
            EventSystem.current.SetSelectedGameObject(gameObject);
        }
    }
}
