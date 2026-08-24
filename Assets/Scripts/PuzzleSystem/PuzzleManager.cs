using System;
using System.Collections.Generic;
using UnityEngine;

public interface IPuzzle
{
    string PuzzleId { get; }
    bool IsCompleted { get; }

    void CompletePuzzle();
}
public class PuzzleManager : MonoBehaviour
{
    public static PuzzleManager Instance { get; private set; }

    private Dictionary<string, IPuzzle> puzzles = new();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void RegisterPuzzle(IPuzzle puzzle)
    {
        puzzles[puzzle.PuzzleId] = puzzle;
    }

    public void CompletePuzzle(IPuzzle puzzle)
    {
        if (!puzzles.ContainsKey(puzzle.PuzzleId))
            return;

        PuzzleCompleted?.Invoke(puzzle.PuzzleId);
    }

    public bool IsCompleted(string id)
    {
        return puzzles.ContainsKey(id);
    }

    public event Action<string> PuzzleCompleted;

    public IPuzzle GetPuzzle(string id)
    {
        foreach (var puzzle in puzzles)
        {
            if (puzzle.Key == id)
                return puzzle.Value;
        }

        return null;
    }
}
