using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Vlajecka : MonoBehaviour, IInteractable
{
    Logik logik;
   
    public int CurrentColor;
    public int CorrectColor;
    public bool IsSetCorrectly() => CurrentColor == CorrectColor;
    public int GetColor() => CurrentColor;

    public void Awake()
    {
        logik = GetComponentInParent<Logik>();
       
    }
    public void Interact()
    {
        ChangeColor();   
    }

    private void ChangeColor()
    {
        CurrentColor = (CurrentColor + 1) % logik.ColorCount;
        Debug.Log(this.name + ": " + CurrentColor);
    }

    public void SetCorrectColor(int color)
    {
        CorrectColor = color;

    }

}
