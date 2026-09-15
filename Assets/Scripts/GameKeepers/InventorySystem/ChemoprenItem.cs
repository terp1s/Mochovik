using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ChemoprenItem : InventoryItem, IPointerClickHandler
{
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
        throw new NotImplementedException();
    }
}