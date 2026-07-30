using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.EventSystems;

public class PuzzlePiece : MonoBehaviour, IPointerClickHandler
{
    public bool isPlaced = false;
    public Vector2Int[] shape;
    public Vector2Int anchorTile;
    public Vector2Int placedAt;

    public Vector2Int span;
    private int minX = 0, maxX = 0, minY = 0, maxY = 0;
    private Vector2 anchorCoord;
    private Vector2 centerCoord;
    public Vector2 Size => GridMaker.TileSize * span;

    private RectTransform rectTransform;
   
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        UpdateMeasures();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (!isPlaced)
            {
                Rotate();
            }
        }
    }

    private void UpdateMeasures()
    {
        span = GetTileSpan();
        anchorCoord = GetAnchorCoordinates();
        centerCoord = GetCenterCoordinates();
    }
    public void Rotate()
    {
        Vector2Int[] result = new Vector2Int[shape.Length];

        for (int i = 0; i < shape.Length; i++)
        {
            Vector2Int v = shape[i];
            result[i] = new Vector2Int(v.y, -v.x);
        }

        anchorTile = new Vector2Int(anchorTile.y, -anchorTile.x);

        shape = result;

        transform.Rotate(0, 0, -90);

        UpdateMeasures();
    }
    public void SnapToPosition(GridTile tile)
    {
        RectTransform fishRect = GetComponent<RectTransform>();

        span = GetTileSpan();
        anchorCoord = GetAnchorCoordinates();
        
        
        transform.parent = tile.transform;
        rectTransform.localPosition = Vector2.zero;
        fishRect.sizeDelta = new Vector2(span.x * GridMaker.TileSize, span.y * GridMaker.TileSize);

        float pivotOffsetX = centerCoord.x - (float)anchorCoord.x;
        float pivotOffsetY = centerCoord.y - (float)anchorCoord.y;

        fishRect.localPosition += new Vector3(pivotOffsetX, pivotOffsetY, 0);
        isPlaced = true;
    }
    private Vector2 GetCenterCoordinates()
    {
        float x = minX +  (float)span.x / 2;
        float y = minY + (float)span.y / 2;

        return new Vector2(x*GridMaker.TileSize, y*GridMaker.TileSize);
    }
    private Vector2 GetAnchorCoordinates()
    {
        float x = GridMaker.TileSize / 2 + GridMaker.TileSize * anchorTile.x;
        float y = GridMaker.TileSize / 2 + GridMaker.TileSize * anchorTile.y;

        return new Vector2(x, y);
    }
    public Vector2Int GetTileSpan()
    {
        minX = 0; maxX = 0; minY = 0; maxY = 0;
        foreach (Vector2Int pos in shape)
        {
            if (pos.x < minX) minX = pos.x;
            if (pos.x > maxX) maxX = pos.x;
            if (pos.y < minY) minY = pos.y;
            if (pos.y > maxY) maxY = pos.y;
        }
        return new Vector2Int(maxX - minX + 1, maxY - minY + 1);
    }

}
