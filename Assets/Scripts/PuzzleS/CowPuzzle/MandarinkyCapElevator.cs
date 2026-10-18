using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.U2D;



public class MandarinkyCapElevator : MonoBehaviour, IItemReceiver, IObjectReceiver, INiche
{
    public enum MandarinkyCapState
    {
        LowerDeck,
        Moving,
        UpperDeck,
    }

    [Header("Settings")]
    public float travelSpeed = 3.0f;
    public Vector2 lowerPosition;
    public Vector2 upperPosition;

    [Header("State")]
    public MandarinkyCapState CurrentState = MandarinkyCapState.LowerDeck;
    public bool IsEmpty => cargo is null;

    private GameObject cargo;

    private Coroutine moveCoroutine;

    private void OnEnable()
    {
        GameEventsManager.instance.puzzleEvents.OnCowsBurned += MoveDown;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.puzzleEvents.OnCowsBurned -= MoveDown;
    }

    public void MoveUp()
    {
        if (CurrentState != MandarinkyCapState.UpperDeck)
        {
            StartMove(upperPosition, MandarinkyCapState.UpperDeck);
        }
    }

    public void MoveDown()
    {
        if (CurrentState != MandarinkyCapState.LowerDeck)
        {
            StartMove(lowerPosition, MandarinkyCapState.LowerDeck);
        }
    }

    private void StartMove(Vector2 target, MandarinkyCapState endState)
    {
        if (moveCoroutine != null) StopCoroutine(moveCoroutine);

        moveCoroutine = StartCoroutine(MoveRoutine(target, endState));
    }

    private IEnumerator MoveRoutine(Vector2 target, MandarinkyCapState endState)
    {
        CurrentState = MandarinkyCapState.Moving;

        while (Vector2.Distance(transform.localPosition, target) > 0.01f)
        {
            transform.localPosition = Vector2.MoveTowards(
                transform.localPosition,
                target,
                travelSpeed * Time.deltaTime
            );
            yield return null;
        }

        transform.localPosition = target;
        CurrentState = endState;

        if (CurrentState == MandarinkyCapState.UpperDeck && !IsEmpty)
        {
            GameEventsManager.instance.puzzleEvents.CargoDelivered(cargo);
        }

        moveCoroutine = null;
    }

    public bool CanReceiveItem(InventoryItem item)
    {
        return CurrentState == MandarinkyCapState.LowerDeck && IsEmpty;
    }

    public void OnReceiveItem(InventoryItem item)
    {
        cargo = item.SpawnInWorld(Vector3.zero, transform);
        
    }
    public bool CanReceiveObject(WorldDraggable item)
    {
        return CurrentState == MandarinkyCapState.LowerDeck && cargo is null;
    }

    public void OnReceiveObject(WorldDraggable item)
    {
        cargo = item.gameObject;
        cargo.transform.SetParent(this.transform);
        cargo.transform.localPosition = Vector3.zero;
    }
   
    public bool CanRemoveObject()
    {
        return CurrentState == MandarinkyCapState.LowerDeck;
    }
    public void OnObjectRemoved()
    {
        cargo = null;
    }

    public void OnObjectHeld()
    {
    }

    public void OnObjectReturned()
    {
        cargo.transform.localPosition = Vector3.zero;

    }
}
