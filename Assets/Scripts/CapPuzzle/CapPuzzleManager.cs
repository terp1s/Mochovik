using Ink.Parsed;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CapPuzzleManager : MonoBehaviour
{
    public List<CapPuzzleAnchor> anchors;
    public bool TryPlaceActiveDrag(CapHolder holder, GameObject drag)
    {
        if (drag is null) { return false; }
        if (!drag.TryGetComponent<CapPuzzlePiece>(out CapPuzzlePiece capPuzzlePiece)) { return false; }

        if (holder.TryAssignCap(capPuzzlePiece))
        {
            capPuzzlePiece.AttachTo(holder);
            CheckWin();
            return true;
        }

        return false;
    }

    public bool CheckWin()
    {
        foreach (var anchor in anchors)
        {
            if (!anchor.IsCompleted())
            {
                return false;
            }
        }
        Debug.Log("Can Puzzle Completed!");
        return true;
    }
}
