using Ink.Parsed;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CapPuzzle : MonoBehaviour, IPuzzle
{
    public List<CapPuzzleAnchor> anchors;
    [SerializeField] private GameObject mushroomDoor;

    public string PuzzleId { get; } = "cap_puzzle";

    public bool IsCompleted { get; private set; } = false;

    private void Start()
    {
        PuzzleManager.Instance.RegisterPuzzle(this);
        Shuffle();
    }

    private void Shuffle()
    {

    }
    public void CheckPuzzleCompletion()
    {
        foreach (var anchor in anchors)
        {
            if (!anchor.IsCompleted())
            {
                //at least one anchor does not have correct succesion of caps
                return;
            }
        }
        Debug.Log("Can Puzzle Completed!");
        
        CompletePuzzle();
    }

    public void CompletePuzzle()
    {
        IsCompleted = true;
        mushroomDoor.SetActive(true);
        PuzzleManager.Instance.CompletePuzzle(this);
    }
}
