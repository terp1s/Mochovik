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
    public Transform LowerDeck;
    public Transform UpperDeck;

    private Vector2 lowerPosition;
    private Vector2 upperPosition;

    [Header("State")]
    public MandarinkyCapState CurrentState = MandarinkyCapState.LowerDeck;
    public bool IsEmpty => cargo is null;

    private GameObject cargo;
    private Animator animator;
    private Coroutine moveCoroutine;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void Start()
    {
        lowerPosition = LowerDeck.position;
        upperPosition = UpperDeck.position;
    }
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
        
        if (CurrentState == MandarinkyCapState.LowerDeck)
        {
            animator.SetTrigger("Up");
        }
    }

    public void MoveDown()
    {
        if (CurrentState == MandarinkyCapState.UpperDeck)
        {
            animator.SetTrigger("Down");
        }
    }
   

    public void ArrivedAtUpperDeck()
    {
        CurrentState = MandarinkyCapState.UpperDeck;

        if (CurrentState == MandarinkyCapState.UpperDeck && !IsEmpty)
        {
            GameEventsManager.instance.puzzleEvents.CargoDelivered(cargo);
        }
    }

    public void ArrivedAtLowerDeck()
    {
        CurrentState = MandarinkyCapState.LowerDeck;
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
