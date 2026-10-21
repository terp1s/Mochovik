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
    public Transform placementPoint;

    private Vector2 lowerPosition;
    private Vector2 upperPosition;

    [Header("State")]
    public MandarinkyCapState CurrentState = MandarinkyCapState.LowerDeck;
    public bool IsEmpty => cargo == null;

    private GameObject cargo;
    private Animator animator;
    private Coroutine moveCoroutine;
    private int originalCargoSortIndex;

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
        cargo.GetComponent<SpriteRenderer>().sortingOrder = 6;
        AlignCargo();
        
    }
    public bool CanReceiveObject(WorldDraggable item)
    {
        return CurrentState == MandarinkyCapState.LowerDeck && cargo is null;
    }

    public void OnReceiveObject(WorldDraggable item)
    {
        originalCargoSortIndex = item.GetComponent<SpriteRenderer>().sortingOrder;
        cargo = item.gameObject;
        cargo.transform.SetParent(this.transform);
        AlignCargo();
        cargo.GetComponent<SpriteRenderer>().sortingOrder = 6;
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
        cargo.GetComponent<SpriteRenderer>().sortingOrder = originalCargoSortIndex; 
    }

    public void OnObjectReturned()
    {
        AlignCargo();

    }

    private void AlignCargo()
    {
        SpriteRenderer sr = cargo.GetComponent<SpriteRenderer>();

        if (sr == null)
            return;

        // Find the bottom-center of the item's visible sprite
        Vector3 bottomCenter = new Vector3(
            sr.bounds.center.x,
            sr.bounds.min.y,
            sr.bounds.center.z
        );

        // Move the item so its bottom-center meets the placement point
        cargo.transform.position += placementPoint.position - bottomCenter;
    }
}
