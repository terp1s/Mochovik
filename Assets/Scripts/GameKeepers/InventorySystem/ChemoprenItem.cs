using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChemoprenItem : InventoryItem, IPointerClickHandler
{
    [SerializeField] GameObject Instructions;
    

    private void Start()
    {
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        ShowInstructions();
    }

    public void UtilizeOn()
    {
        Debug.Log(gameObject.name + " is being used");

        Destroy(gameObject);
    }

    public void ShowInstructions()
    {
        descriptionPanel.Show(Instructions);
    }
}