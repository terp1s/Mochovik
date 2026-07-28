using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public enum MushroomState { PlaceAndColor, Color, None }
public class Logik : MonoBehaviour
{
    public Vlajecka[] Vlajecky;
    public List<MushroomSpawn> Spawns = new List<MushroomSpawn>();
    public int ColorCount;
    public int[] kod;
    public int kodLength;
    private int _roundCounter;
    public int MaxRound;

    private void Start()
    {
        StartGame();
    }

    public void StartGame()
    {
        _roundCounter = 0;

        CreateCode();
    }

    void CreateCode()
    {
        kod = new int[kodLength];


        System.Random random = new System.Random();

        for (int i = 0; i < kodLength; i++)
        {
            kod[i] = random.Next(ColorCount);
            Vlajecky[i].SetCorrectColor(kod[i]);
        }

        //Shuffle(ref kod);
    }

    private void Shuffle(ref int[] array)
    {
        System.Random rng = new System.Random();
        int n = array.Length;
        while (n > 1)
        {
            int k = rng.Next(n--);
            int temp = array[n];
            array[n] = array[k];
            array[k] = temp;
        }
    }

    public void Round()
    {

        (int right, int wrong) = Evaluate();

        Debug.Log($"correct: {right}; wrong: {wrong}");

        if (right == kodLength)
        {
            Win();
        }
        else if(_roundCounter == MaxRound)
        {
            Lose();
        }

        _roundCounter++;

        PrintResult(right, wrong);
    }

    private (int, int) Evaluate()
    {
        int RightColorWrongPlace = 0;
        int RightColorRightPlace = 0;

        for (int i = 0; i < kodLength; i++)
        {
            Vlajecka vlajecka = Vlajecky[i];
            Vlajecka vlajeckaScript = vlajecka.GetComponent<Vlajecka>();

            if (vlajeckaScript.IsSetCorrectly())
            {
                RightColorRightPlace++;
            }
            else if (kod.Contains(vlajeckaScript.GetColor()))
            {
                RightColorWrongPlace++;
            }
        }

        return (RightColorRightPlace, RightColorWrongPlace);
    }

    private void Win()
    {
        foreach (var vlajecka in Vlajecky)
        {
            vlajecka.enabled = false;
        }

        Debug.Log("win");
    }
    private void Lose()
    {
        Debug.Log("lose");

    }

    private void PrintResult(int right, int wrong)
    {
        for (int i = 0; i < right; i++)
        {
            Spawns[i].Spawn(MushroomState.PlaceAndColor);
        }
        for (int i = right; i < right + wrong; i++)
        {
            Spawns[i].Spawn(MushroomState.Color);
        }
        for(int i = right + wrong; i < Spawns.Count; i++)
        {
            Spawns[i].Spawn(MushroomState.None);
        }

    }
}
