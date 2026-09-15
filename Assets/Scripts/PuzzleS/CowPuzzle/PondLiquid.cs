using UnityEngine;
using UnityEngine.U2D;

public class PondLiquid : MonoBehaviour
{
    [SerializeField] private Sprite water;
    [SerializeField] private Sprite liquidGlue;
    [SerializeField] private Sprite stickyGlue;
    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = water;
    }

    public void ChangeToLiquidGlue()
    {
        spriteRenderer.sprite = liquidGlue;
    }

    public void Drain()
    {
        spriteRenderer.sprite = null;
    }

    public void ChangeToStickyGlue()
    {
        spriteRenderer.sprite = stickyGlue;
    }
}
