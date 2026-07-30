using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Net.NetworkInformation;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SardinePuzzleManager : MonoBehaviour
{
    public GridTile[,] gridTiles;
    public Vector2Int currentHoverCoords;
    private bool isHovering;
    public GridMaker Grid;
    public Image image;
    public Sprite WinCan;

    private void OnEnable()
    {
        if (Grid is not null)
        {

            gridTiles = Grid.GetGrid();

            if (gridTiles is null)
            {
                Debug.LogError("GridMaker returned a null grid!");
            }
        }
        else
        {
            Debug.LogError("Grid reference is missing on SardinePuzzleManager!");
        }
    }

    public void HoverTile(Vector2Int coords)
    {
        isHovering = true;
        currentHoverCoords = coords;
        Debug.Log("Hovering over " + coords.x + ", " + coords.y);
    }

    public void StopHovering()
    {
        isHovering = false;
        Debug.Log("Stopped Hovering");
    }

    public void TryPlaceActiveDrag(Vector2Int coords, GameObject fish)
    {
        if (fish is null) { return; }
        if (!fish.TryGetComponent<PuzzlePiece>(out PuzzlePiece puzzlePiece)) { return; }
        if (!isHovering) return;

        if (CanFit(puzzlePiece, currentHoverCoords))
        {
            puzzlePiece.SnapToPosition(gridTiles[currentHoverCoords.x, currentHoverCoords.y]);
            puzzlePiece.isPlaced = true;
            puzzlePiece.placedAt = coords;
          
            Debug.Log($"attached fish to {currentHoverCoords.x}, {currentHoverCoords.y}");

            
            MarkTiles(currentHoverCoords, puzzlePiece);
        }

        CheckWin();
    }

    private void MarkTiles(Vector2Int coords, PuzzlePiece piece)
    {
        foreach (Vector2Int offset in piece.shape)
        {
            Vector2Int tilePos = offset + coords - piece.anchorTile;

            gridTiles[tilePos.x, tilePos.y].PlacePiece(piece);
        }
    }


    private bool CanFit(PuzzlePiece piece, Vector2Int coords)
    {
        foreach (var coordinate in piece.shape)
        {
            int newX = coords.x + coordinate.x - piece.anchorTile.x;
            int newY = coords.y + coordinate.y - piece.anchorTile.y;

            if (gridTiles[newX, newY].IsOccupied())
            {
                return false;
            }
        }
        return true;
    }

    public void RemovePiece(PuzzlePiece piece)
    {
        if (!piece.isPlaced) return;

        foreach (Vector2Int offset in piece.shape)
        {
            int tileX = piece.placedAt.x + offset.x - piece.anchorTile.x;
            int tileY = piece.placedAt.y + offset.y - piece.anchorTile.y;

            gridTiles[tileX, tileY].RemovePiece();
        }

        piece.isPlaced = false;
    }

    private bool IsSolved()
    {
        foreach (var tile in gridTiles)
        {
            if (!tile.IsOccupied())
            {
                return false;
            }
        }
        return true;
    }

    public void CheckWin()
    {
        if (IsSolved())
        {
            image.sprite = WinCan;

            Grid.gameObject.SetActive(false);
        }
       
    }
}
