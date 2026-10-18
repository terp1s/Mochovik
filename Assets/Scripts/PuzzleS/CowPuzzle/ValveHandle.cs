using UnityEngine;
using UnityEngine.EventSystems;
public class ValveHandle : WorldDraggable, IPointerClickHandler
{
    private Animator animator;
    private bool placedOnValve = false;
    private bool hasTurned = false;
    [SerializeField] private CircleCollider2D circleCollider;

    private new void Awake()
    {
        base.Awake();

        animator = GetComponent<Animator>();
        circleCollider = GetComponent<CircleCollider2D>();        
    }

    public void Place()
    {
        animator.SetTrigger("Placed");
        GetComponent<CollectOnClick>().enabled = false;
        placedOnValve = true;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (placedOnValve && !hasTurned)
        {
            Debug.Log("Valve handle has begun turning!");

            hasTurned = true;
            GameEventsManager.instance.puzzleEvents.ValveHandleTurned();
            animator.SetTrigger("Turn");
        }
    }

    public void UpdateColliderSize()
    {
        Vector2 spriteSize = spriteRenderer.sprite.bounds.size;

        circleCollider.radius = Mathf.Max(spriteSize.x, spriteSize.y) / 2f;
        circleCollider.offset = spriteRenderer.sprite.bounds.center;
    }

    protected override bool CanStartDrag(PointerEventData eventData)
    {
        return !placedOnValve;
    }
}
