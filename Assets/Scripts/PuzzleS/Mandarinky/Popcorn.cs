using UnityEngine;
using UnityEngine.EventSystems;

public class Popcorn : WorldDraggable, IEdible
{
    public bool IsDraggable = true;
    public bool IsGrown = false;

    [SerializeField] Animator animator;
    [SerializeField] CircleCollider2D circleCollider;


    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
        circleCollider = GetComponent<CircleCollider2D>();
    }
    protected override bool CanStartDrag(PointerEventData eventData)
    {
        return IsDraggable;
    }
    
    protected override void OnDragStart()
    {
        base.OnDragStart();
        originalPosition = transform.position;


    }


    public void UpdateColliderSize()
    {
        Vector2 spriteSize = spriteRenderer.sprite.bounds.size;

        circleCollider.radius = Mathf.Max(spriteSize.x, spriteSize.y) / 2f;
        circleCollider.offset = spriteRenderer.sprite.bounds.center;
    }

    public void Grow()
    {
        animator.SetTrigger("Grow");
    }

    public void Lock()
    {
        IsDraggable = false;
    }

    public void Unlock()
    {
        IsDraggable = true;
    }

    public void SetToGrown()
    {
        IsGrown = true;
    }
}
