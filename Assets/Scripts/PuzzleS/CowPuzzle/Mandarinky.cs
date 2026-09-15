using UnityEngine;
using UnityEngine.EventSystems;

public class Mandarinky : MonoBehaviour, IPointerClickHandler
{
    enum MandarinkyState
    {
        Sealed,
        Cut,
    }

    MandarinkyState currentState = MandarinkyState.Sealed;
    MandarinkyCap cap;

    private void Awake()
    {
        cap = GetComponentInChildren<MandarinkyCap>();
    }

    private void OnEnable()
    {
        GameEventsManager.instance.puzzleEvents.OnJarPlaced += PlacedJar;
    }

    private void OnDisable()
    {
        if (GameEventsManager.instance != null)
        {
            GameEventsManager.instance.puzzleEvents.OnJarPlaced -= PlacedJar;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (currentState == MandarinkyState.Sealed)
        {
            currentState = MandarinkyState.Cut;
        }
    }

    public void PlacedJar(string drop, MasonJar jar)
    {
        if(drop is not "mandarinky" ||jar is null) { return; }

        if (jar.IsFull && currentState == MandarinkyState.Sealed)
        {
            Smoke();
        }

        else if (jar.IsFull && currentState == MandarinkyState.Cut)
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
        cap.MoveUp();
    }

}
