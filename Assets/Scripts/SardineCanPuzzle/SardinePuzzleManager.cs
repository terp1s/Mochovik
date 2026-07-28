using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class SardinePuzzleManager : MonoBehaviour, IInteractable
{
    public GridTile[,] gridTiles;
    public Vector2Int currentHoverCoords;
    private bool isHovering;
    public GridMaker Grid;

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

    public void Interact()
    {
        
        TryPlaceActiveDrag();
        
    }

    private void TryPlaceActiveDrag()
    {
        /*
        UIDragInstance drag = FindObjectOfType<UIDragInstance>();
        if (drag == null || !isHovering) return;
        ItemData genericData = drag.GetData();

        if (genericData is PuzzlePieceData pieceData)
        {
            if (CanFit(pieceData, currentHoverCoords))
            {
                GameObject piecePrefab = drag.GetData().uiDragPrefab;


                GameObject finalPiece = Instantiate(piecePrefab, this.transform);
                PuzzlePiece pieceScript = finalPiece.GetComponent<PuzzlePiece>();

                pieceScript.SnapToPosition(gridTiles[currentHoverCoords.x, currentHoverCoords.y].transform.position);
                pieceScript.isPlaced = true;

                MarkTiles(currentHoverCoords, pieceScript);

                Destroy(drag.gameObject);
            }
        }
        */
    }
    
    private void MarkTiles(Vector2Int coords, PuzzlePiece piece)
    {
        foreach (Vector2Int offset in piece.shape)
        {
            Vector2Int tilePos = offset + coords - piece.anchor;

            gridTiles[tilePos.x, tilePos.y].PlacePiece(piece);
        }
    }


    private bool CanFit(PuzzlePieceData pieceData, Vector2Int coords)
    {
        foreach (var coordinate in pieceData.shape)
        {
            int newX = coords.x + coordinate.x - pieceData.anchor.x;
            int newY = coords.y + coordinate.y - pieceData.anchor.y;

            if (gridTiles[newX, newY].IsOccupied())
            {
                return false;
            }
        }
        return true;
    }
   

    public void RemovePiece(Vector2Int coords)
    {
        PuzzlePiece occupyingPiece = gridTiles[coords.x, coords.y].currentPiece;

        foreach (Vector2Int offset in occupyingPiece.shape)
        {
            Vector2Int tilePos = occupyingPiece.anchor + offset + coords; 

            gridTiles[tilePos.x, tilePos.y].RemovePiece();
        }
    }

    private bool IsSolved()
    {
        foreach(var tile in gridTiles)
        {
            if (!tile.IsOccupied())
            {
                return false;
            }
        }
        return true;
    }  
}
