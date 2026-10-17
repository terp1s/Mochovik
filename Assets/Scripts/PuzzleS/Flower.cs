using UnityEngine;
using UnityEngine.EventSystems;

public class Flower : WorldDraggable, IEdible
{
    protected override bool CanStartDrag(PointerEventData eventData)
    {
        if(CurrentNiche is not null)
        {
            return CurrentNiche.CanRemoveObject();
        }

        return true;
    }
}
