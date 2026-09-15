using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSelect : MonoBehaviour, ISelectHandler
{
    public GameObject SelectionBrackets;
    [SerializeField] bool isDefault = false;
    private void Awake()
    {
        if (isDefault)
        {
            GetComponent<UnityEngine.UI.Button>().Select();
        }
    }
    public void OnSelect(BaseEventData eventData)
    {
        SelectionBrackets.transform.SetParent(transform);
        SelectionBrackets.transform.localPosition = Vector3.zero;
    }
}
