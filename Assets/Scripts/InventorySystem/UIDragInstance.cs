/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UIDragInstance : MonoBehaviour
{
    private ItemData data;
    private Canvas canvas;
    private RectTransform rectTransform;

    public void Setup(ItemData itemData, Canvas parentCanvas)
    {
        data = itemData;
        canvas = parentCanvas;
        rectTransform = GetComponent<RectTransform>();

        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    private void Update()
    {
        Vector2 movePos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            Input.mousePosition,
            canvas.worldCamera,
            out movePos);

        rectTransform.anchoredPosition = movePos;
    }

    public ItemData GetData() => data;
}
*/

using UnityEngine;

public class UIDragInstance : MonoBehaviour
{
    private ItemData data;

    public void Setup(ItemData itemData)
    {
        data = itemData;
    }

    public ItemData GetData() => data;
}