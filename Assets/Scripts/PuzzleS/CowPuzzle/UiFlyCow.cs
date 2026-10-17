using UnityEngine;

public class UiFlyCow : MonoBehaviour
{
    private RectTransform rectTransform;
    private RectTransform boundary;
    private Vector2 targetPos;
    public float speed = 50f;

    public void Setup(RectTransform jarBounds)
    {
        rectTransform = GetComponent<RectTransform>();
        boundary = jarBounds;
        PickNewTarget();
    }

    void Update()
    {
        rectTransform.anchoredPosition = Vector2.MoveTowards(
            rectTransform.anchoredPosition,
            targetPos,
            speed * Time.deltaTime
        );

        if (Vector2.Distance(rectTransform.anchoredPosition, targetPos) < 2f)
        {
            PickNewTarget();
        }
    }

    void PickNewTarget()
    {
        if (boundary == null) return;

        float width = boundary.rect.width * 0.3f;
        float height = boundary.rect.height * 0.3f;

        targetPos = new Vector2(
            Random.Range(-width, width),
            Random.Range(-height, height)
        );
    }
}