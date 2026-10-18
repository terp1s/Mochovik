using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tongue : MonoBehaviour, INiche
{
    public enum TongueState { In, Moving, Out }
    [SerializeField] private float autoRetractDelay = 5f;

    public GameObject Cargo;

    [SerializeField] Transform tongueTip;

    private Animator animator;
    private TongueState state;
    private Coroutine timeoutCoroutine;

    private ChanterelleDoor door;
    private bool isCargoBeingDragged = false;
    public bool IsOccupied => Cargo != null;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }


    public void SetState(TongueState newState)
    {
        state = newState;

        if (Cargo != null)
        {
            Collider2D cargoCol = Cargo.GetComponent<Collider2D>();
            if (cargoCol != null)
            {
                cargoCol.enabled = (state == TongueState.Out);
            }
        }

        if (state == TongueState.Out /*&& Cargo != null && !isCargoBeingDragged*/)
        {
            StartRetractTimer();
        }
    }


    public void TongueOut(GameObject newCargo, ChanterelleDoor doorSource)
    {
        door = doorSource;
        isCargoBeingDragged = false;

        //new cargo
        if (newCargo != null && Cargo == null)
        {
            Cargo = newCargo;

            Cargo.transform.SetParent(tongueTip, false);
            Cargo.transform.localPosition = Vector3.zero;

            AlignCargoBottomToTip();


            if (Cargo.TryGetComponent(out WorldDraggable draggable))
            {
                draggable.SetNiche(this);
            }
            if (Cargo.TryGetComponent(out CollectOnClick collectible))
            {
                collectible.CurrentNiche = this;
            }

            Collider2D cargoCol = Cargo.GetComponent<Collider2D>();
            if (cargoCol != null) cargoCol.enabled = false;

        }

        animator.SetTrigger("TongueOut");
    }

    private void AlignCargoBottomToTip()
    {
        if (Cargo == null || tongueTip == null) return;

        SpriteRenderer[] renderers = Cargo.GetComponentsInChildren<SpriteRenderer>();
        if (renderers.Length == 0) return;

        float lowestY = float.MaxValue;
        foreach (var sr in renderers)
        {
            if (sr.bounds.min.y < lowestY)
            {
                lowestY = sr.bounds.min.y;
            }
        }

        float offsetUp = tongueTip.position.y - lowestY;

        Cargo.transform.position += new Vector3(0, offsetUp, 0);
    }

    public void TongueIn()
    {
        animator.SetTrigger("TongueIn");
    }

    private void StartRetractTimer()
    {
        StopRetractTimer();
        timeoutCoroutine = StartCoroutine(RetractTimeoutRoutine());
    }

    private void StopRetractTimer()
    {
        if (timeoutCoroutine != null)
        {
            StopCoroutine(timeoutCoroutine);
            timeoutCoroutine = null;
        }
    }

    private IEnumerator RetractTimeoutRoutine()
    {
        yield return new WaitForSeconds(autoRetractDelay);

        if (Cargo != null && !isCargoBeingDragged && state == TongueState.Out)
        {
            if (door != null)
            {
                door.OnCargoExpired();
            }
        }
        else if(Cargo == null)
        {
            door.OnCargoTaken();
        }
    }
    public bool CanRemoveObject()
    {
        return state == TongueState.Out;
    }

    public void OnObjectHeld()
    {
        isCargoBeingDragged = true;
        StopRetractTimer(); 
    }

    public void OnObjectReturned()
    {
        isCargoBeingDragged = false;
        AlignCargoBottomToTip();
        StartRetractTimer(); 
    }

    public void OnObjectRemoved()
    {
        isCargoBeingDragged = false;
        StopRetractTimer();
        Cargo = null;

        if (door != null)
        {
            door.OnCargoTaken();
        }
    }
    public bool CanReceiveObject(WorldDraggable obj)
    {
        //one way niche
        return false;
    }

    public void OnReceiveObject(WorldDraggable obj)
    {
    }

    public void ClearCargoReference()
    {
        Cargo = null;
    }

    public void RemoveCargo()
    {
        Destroy(Cargo);
        Cargo = null;
    }
}