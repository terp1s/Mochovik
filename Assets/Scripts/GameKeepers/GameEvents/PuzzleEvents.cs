
using System;
using UnityEngine;

public class PuzzleEvents
{
    public event Action<string, GameObject> OnCargoDelivered;
    public void CargoDelivered(string id, GameObject cargo)
    {
        OnCargoDelivered?.Invoke(id, cargo);
    }

    public event Action<string, MasonJar> OnJarPlaced;
    public void JarPlaced(string id, MasonJar jar)
    {
        OnJarPlaced?.Invoke(id, jar);
    }

    public event Action OnCowsBurned;

    public void CowsBurned()
    {
        OnCowsBurned?.Invoke();
    }
}