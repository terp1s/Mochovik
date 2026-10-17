using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class Mandarinky : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] MandarinkyHole hole;
    
    MandarinkyCapElevator capElevator;
    MandarinkyCap cap;
    

    private void Awake()
    {
        capElevator = GetComponentInChildren<MandarinkyCapElevator>();
        cap = GetComponentInChildren<MandarinkyCap>();
    }

    private void OnEnable()
    {
        GameEventsManager.instance.puzzleEvents.OnCowsFedInMandarinky += CowsFed;
    }

    private void OnDisable()
    {
        if (GameEventsManager.instance != null)
        {
            GameEventsManager.instance.puzzleEvents.OnCowsFedInMandarinky -= CowsFed;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        hole.gameObject.SetActive(true);

    }

    public void CowsFed()
    {
        if (cap.IsSealed)
        {
            Smoke();
        }
        else
        {
            Elevator();
        }
    }

    void Smoke()
    {
        Debug.Log("Smoke effect triggered!");
    }

    void Elevator()
    {
        capElevator.MoveUp();
    }

}
