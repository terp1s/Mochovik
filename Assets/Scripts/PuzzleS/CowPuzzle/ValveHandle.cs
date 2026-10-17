using UnityEngine;
using UnityEngine.EventSystems;
public class ValveHandle : MonoBehaviour, IPointerClickHandler
{
    private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private CircleCollider2D circleCollider;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        circleCollider = GetComponent<CircleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        
    }

    public void Place()
    {
        animator.SetTrigger("Placed");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Valve handle has begun turning!");
        animator.SetTrigger("Turn");
    }

    public void FinishedTurning()
    {
        Debug.Log("Valve handle finished turning!");
        GameEventsManager.instance.puzzleEvents.ValveHandleTurned();
    }

    public void UpdateColliderSize()
    {
        Vector2 spriteSize = spriteRenderer.sprite.bounds.size;

        circleCollider.radius = Mathf.Max(spriteSize.x, spriteSize.y) / 2f;
        circleCollider.offset = spriteRenderer.sprite.bounds.center;
    }
}
