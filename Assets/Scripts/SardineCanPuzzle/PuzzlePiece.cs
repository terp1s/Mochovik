using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzlePiece : MonoBehaviour
{
    public bool isPlaced = false;
    public Vector2Int[] shape;
    public Vector2Int anchor;

    private RectTransform rectTransform;
   
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    public void Rotate()
    {

    }
    public void SnapToPosition(Vector2 screenPosition)
    {
        rectTransform.position = screenPosition;
        isPlaced = true;
    }
}
