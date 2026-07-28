using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzlePiece : MonoBehaviour
{
    public PuzzlePieceData data;
    public bool isPlaced = false;
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
