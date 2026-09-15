using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

public class MandarinkyCap : MonoBehaviour, IDropHandler
{
    public enum MandarinkyCapState
    {
        LowerDeck,
        Moving,
        UpperDeck,
    }

    [Header("Settings")]
    public float travelSpeed = 2.0f;
    public Vector2 lowerPosition;
    public Vector2 upperPosition;

    [Header("State")]
    public MandarinkyCapState currentState = MandarinkyCapState.LowerDeck;

    private GameObject cargo;
    public System.Action<GameObject> OnCargoDelivered;

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
        if (currentState != MandarinkyCapState.UpperDeck)
        {
            StartMove(upperPosition, MandarinkyCapState.UpperDeck);
        }
    }

    public void MoveDown()
    {
        if (currentState != MandarinkyCapState.LowerDeck)
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
        currentState = MandarinkyCapState.Moving;

        while (Vector2.Distance(transform.position, target) > 0.01f)
        {
            transform.position = Vector2.MoveTowards(
                transform.position,
                target,
                travelSpeed * Time.deltaTime
            );
            yield return null;
        }

        transform.position = target;
        currentState = endState;

        if (currentState == MandarinkyCapState.UpperDeck && cargo != null)
        {
            OnCargoDelivered?.Invoke(cargo);
            cargo = null;
        }

        moveCoroutine = null;
    }


    public bool BoardCargo(GameObject item)
    {
        if(currentState == MandarinkyCapState.LowerDeck && cargo is null)
        {
            cargo = item;
            item.transform.SetParent(this.transform);
            return true;
        }
        return false;
    }

    public void OnDrop(PointerEventData eventData)
    {
        BoardCargo(eventData.pointerDrag);

    }
}
