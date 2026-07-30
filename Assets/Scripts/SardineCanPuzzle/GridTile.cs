using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GridTile : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDropHandler
{
    public Vector2Int coordinates;

    public PuzzlePiece currentPiece;
    public SardinePuzzleManager manager;

    public bool IsOccupied()
    {
        return currentPiece is not null;
    }

    public void PlacePiece(PuzzlePiece piece)
    {
        currentPiece = piece;   
    }

    public void RemovePiece()
    {
        currentPiece = null;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        manager.HoverTile(coordinates);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        manager.StopHovering();
    }

    public void OnDrop(PointerEventData eventData)
    {
        manager.TryPlaceActiveDrag(coordinates, eventData.pointerDrag);
    }
}