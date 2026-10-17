using System.Collections.Generic;
using UnityEngine;

public class CapPuzzleRandomizer : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CapPuzzle capPuzzle;
    [SerializeField] private List<CapPuzzlePiece> allPieces;

    [Tooltip("Optional: If left empty, pieces without a ChildHolder are automatically detected as bottom caps.")]
    [SerializeField] private List<CapPuzzlePiece> bottomPiecesOverride;

    [Header("Settings")]
    [SerializeField] private bool randomizeOnStart = true;

    private void Awake()
    {
        if (capPuzzle == null)
            capPuzzle = GetComponent<CapPuzzle>();
    }

    private void Start()
    {
        if (randomizeOnStart)
        {
            RandomizePuzzle();
        }
    }

    [ContextMenu("Randomize Puzzle")]
    public void RandomizePuzzle()
    {
        if (capPuzzle == null || capPuzzle.anchors == null || capPuzzle.anchors.Count == 0)
        {
            Debug.LogWarning("CapPuzzleRandomizer: No CapPuzzle or anchors found!", this);
            return;
        }

        if (allPieces == null || allPieces.Count == 0)
        {
            allPieces = new List<CapPuzzlePiece>(FindObjectsByType<CapPuzzlePiece>(FindObjectsSortMode.None));
        }

        if (allPieces.Count == 0)
        {
            Debug.LogWarning("CapPuzzleRandomizer: No CapPuzzlePiece items found to randomize!", this);
            return;
        }

        DetachAllCaps();

        int attempts = 0;
        bool isSolved;

        do
        {
            DistributeCapsWithBottomsPreserved();
            isSolved = CheckIfSolved();
            attempts++;

        } while (isSolved && attempts < 20);

        capPuzzle.CheckPuzzleCompletion();
    }

    private void DistributeCapsWithBottomsPreserved()
    {
        List<CapPuzzlePiece> bodyPool = new List<CapPuzzlePiece>();
        List<CapPuzzlePiece> bottomPool = new List<CapPuzzlePiece>();

        foreach (var piece in allPieces)
        {
            if (IsBottomPiece(piece))
            {
                bottomPool.Add(piece);
            }
            else
            {
                bodyPool.Add(piece);
            }
        }

        ShuffleList(bodyPool);
        ShuffleList(bottomPool);

        int anchorCount = capPuzzle.anchors.Count;
        int totalCaps = allPieces.Count;

        int baseCount = totalCaps / anchorCount;
        int remainder = totalCaps % anchorCount;

        int[] countsPerRope = new int[anchorCount];
        for (int i = 0; i < anchorCount; i++)
        {
            countsPerRope[i] = baseCount + (i < remainder ? 1 : 0);
        }
        ShuffleArray(countsPerRope);

        for (int ropeIdx = 0; ropeIdx < anchorCount; ropeIdx++)
        {
            CapPuzzleAnchor anchor = capPuzzle.anchors[ropeIdx];
            CapHolder currentHolder = anchor.GetComponent<CapHolder>();

            int countForThisRope = countsPerRope[ropeIdx];
            if (countForThisRope <= 0 || currentHolder == null) continue;

            int bodySlots = countForThisRope - 1;

            for (int j = 0; j < bodySlots; j++)
            {
                CapPuzzlePiece bodyPiece = PopPiece(bodyPool, bottomPool);
                if (bodyPiece == null || currentHolder == null) break;

                currentHolder.cap = bodyPiece;
                bodyPiece.AttachTo(currentHolder);

                currentHolder = GetChildHolder(bodyPiece);
            }

            if (currentHolder != null)
            {
                CapPuzzlePiece bottomPiece = PopPiece(bottomPool, bodyPool);
                if (bottomPiece != null)
                {
                    currentHolder.cap = bottomPiece;
                    bottomPiece.AttachTo(currentHolder);
                }
            }
        }
    }

  
    private bool IsBottomPiece(CapPuzzlePiece piece)
    {
        if (bottomPiecesOverride != null && bottomPiecesOverride.Count > 0)
        {
            return bottomPiecesOverride.Contains(piece);
        }

        CapHolder holder = GetChildHolder(piece);
        return holder == null;
    }

    private CapHolder GetChildHolder(CapPuzzlePiece piece)
    {
        if (piece.ChildHolder != null) return piece.ChildHolder;
        return piece.GetComponentInChildren<CapHolder>();
    }

    private CapPuzzlePiece PopPiece(List<CapPuzzlePiece> primaryPool, List<CapPuzzlePiece> fallbackPool)
    {
        if (primaryPool.Count > 0)
        {
            CapPuzzlePiece piece = primaryPool[primaryPool.Count - 1];
            primaryPool.RemoveAt(primaryPool.Count - 1);
            return piece;
        }

        if (fallbackPool.Count > 0)
        {
            CapPuzzlePiece piece = fallbackPool[fallbackPool.Count - 1];
            fallbackPool.RemoveAt(fallbackPool.Count - 1);
            return piece;
        }

        return null;
    }

    private void DetachAllCaps()
    {
        foreach (var piece in allPieces)
        {
            if (piece.ParentHolder != null)
            {
                piece.ParentHolder.RemoveCap();
            }
            if (piece.ChildHolder != null)
            {
                piece.ChildHolder.RemoveCap();
            }
            piece.Detach();
        }

        foreach (var anchor in capPuzzle.anchors)
        {
            CapHolder anchorHolder = anchor.GetComponent<CapHolder>();
            if (anchorHolder != null)
            {
                anchorHolder.RemoveCap();
            }
        }
    }

    private bool CheckIfSolved()
    {
        foreach (var anchor in capPuzzle.anchors)
        {
            if (!anchor.IsCompleted())
                return false;
        }
        return true;
    }

    private void ShuffleList<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int rnd = Random.Range(0, i + 1);
            (list[i], list[rnd]) = (list[rnd], list[i]);
        }
    }

    private void ShuffleArray<T>(T[] array)
    {
        for (int i = array.Length - 1; i > 0; i--)
        {
            int rnd = Random.Range(0, i + 1);
            (array[i], array[rnd]) = (array[rnd], array[i]);
        }
    }
}