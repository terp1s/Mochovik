using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public enum CowState { Wandering, GoingToPond, Landing, Drinking, Fleeing, Stuck, Eating}

public class Cow : WorldDraggable, IPointerClickHandler, IObjectReceiver
{
    public FlyMovement movement;
    private Animator animator;

    [SerializeField] private CowState currentState = CowState.Wandering;
    public CowState CurrentState => currentState;
    public bool IsInJar = false;

    private new void Awake()
    {
        base.Awake();
        movement = GetComponent<FlyMovement>();
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        GameEventsManager.instance.puzzleEvents.OnGlueDried += GlueDried;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.puzzleEvents.OnGlueDried -= GlueDried;
    }

    public void SetState(CowState newState)
    {
        if (currentState == newState) return; 
       
        currentState = newState;
        UpdateAnimation(newState);
    }

    public AnimatorStateInfo GetAnimatorStateInfo()
    {
        return animator.GetCurrentAnimatorStateInfo(0);
    }

    private void UpdateAnimation(CowState state)
    {
        if (animator == null) return;

        animator.ResetTrigger("Landing");
        animator.ResetTrigger("Drinking");
        animator.ResetTrigger("Stuck");
        animator.ResetTrigger("Wandering");
        animator.ResetTrigger("Fleeing");
        animator.ResetTrigger("Eating");


        animator.SetTrigger(state.ToString());
    }



    public void OnPointerClick(PointerEventData eventData)
    {
        if(currentState != CowState.Stuck && currentState != CowState.Eating)
        {
            movement.OnClick(eventData);
        }
    }

    protected override bool CanStartDrag(PointerEventData eventData)
    {
        return currentState == CowState.Stuck || currentState == CowState.Eating; 
    }

    protected override void OnDragEnd()
    {
        base.OnDragEnd();

        if(IsInJar)
        {
            transform.localPosition = new Vector3(0, 0, -0.1f);
        }
        else
        {
            transform.position = originalPosition;
        }

    }

    public void Capture(Transform parent, CowRoamingArea area)
    {
        transform.SetParent(parent);
        transform.localPosition = new Vector3(0, 0, -0.1f);
        transform.localScale *= 0.15f;
        IsInJar = true;
        movement.roamingArea = area;
        GetComponent<Collider2D>().enabled = false;
        movement.enabled = true;
        movement.PickNewTarget();
    }

    public bool CanReceiveObject(WorldDraggable item)
    {
        return item.gameObject.TryGetComponent<Popcorn>(out Popcorn pop) && currentState == CowState.Stuck && pop.IsGrown;
    }

    public void OnReceiveObject(WorldDraggable item)
    {
        Destroy(item.gameObject);
        SetState(CowState.Eating);
    }

    private void GlueDried()
    {
        if(currentState == CowState.Drinking)
        {
            SetState(CowState.Stuck);
        }
    }
}
