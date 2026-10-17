using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class SpriteToImageBridge : MonoBehaviour
{
    private Image uiImage;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        uiImage = GetComponent<Image>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = false;
        }
    }

    private void LateUpdate()
    {
        if (spriteRenderer != null && uiImage != null)
        {
            uiImage.sprite = spriteRenderer.sprite;
        }
    }
}