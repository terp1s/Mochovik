using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public enum CowState { Wandering, GoingToPond, Drinking, Fleeing, Stuck}

public class Cow : WorldDraggable, IPointerClickHandler
{
    public FlyMovement movement;
    public CowState currentState = CowState.Wandering;
    public bool IsInJar { get; set; } = false;

    private new void Awake()
    {
        base.Awake();
        movement = GetComponent<FlyMovement>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(currentState != CowState.Stuck)
        {
            movement.OnClick(eventData);
        }
    }

    protected override bool CanStartDrag(PointerEventData eventData)
    {
        return currentState == CowState.Stuck; 
    }

    public void Capture(Transform parent, CowRoamingArea area)
    {
        transform.SetParent(parent);
        transform.localPosition = new Vector3(0, 0, -0.1f);
        transform.localScale *= 0.25f;
        IsInJar = true;
        movement.roamingArea = area;
        GetComponent<Collider2D>().enabled = false;
        movement.enabled = true;
    }
}
