using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Puzzle/Piece")]
public class PuzzlePieceData : ItemData
{
    public Vector2Int[] shape;
    public Vector2Int anchor;
}
