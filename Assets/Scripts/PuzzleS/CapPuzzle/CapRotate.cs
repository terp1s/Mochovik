using UnityEngine;
using UnityEngine.EventSystems;

public class CapRotate : MonoBehaviour, IPointerClickHandler
{
    public int ID;

    [SerializeField] CapRotationManager manager;
    [SerializeField] Sprite capBackSprite;
    private Sprite originalSprite;
    private SpriteRenderer spriteRenderer;
    private bool isRotated = false;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        manager = FindFirstObjectByType<CapRotationManager>();
        originalSprite = spriteRenderer.sprite;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (!isRotated)
        {
            spriteRenderer.sprite = capBackSprite;
        }
        else
        {
            spriteRenderer.sprite = originalSprite;
        }

        isRotated = !isRotated;

        manager.UpdateCapState(this, isRotated);
    }

    public void SpawnValve(GameObject valve)
    {
        Instantiate(valve, transform.position, Quaternion.identity, transform);
    }
}
