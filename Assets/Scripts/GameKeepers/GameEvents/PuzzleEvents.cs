
using System;
using UnityEngine;

public class PuzzleEvents
{
    public event Action<GameObject> OnCargoDelivered;
    public void CargoDelivered(GameObject cargo)
    {
        OnCargoDelivered?.Invoke(cargo);
    }

    public event Action OnCowsFedInMandarinky;

    public void CowsFedInMandarinky()
    {
        OnCowsFedInMandarinky?.Invoke();
    }

    public event Action OnCowsBurned;

    public void CowsBurned()
    {
        OnCowsBurned?.Invoke();
    }

    public event Action OnValveHandleTurned;
    public void ValveHandleTurned()
    {
        OnValveHandleTurned?.Invoke();
    }

    public event Action OnGlueDried;

    public void GlueDried()
    {
        OnGlueDried?.Invoke();
    }

    public event Action OnGlueApplied;

    public void GlueApplied()
    {
        OnGlueApplied?.Invoke();
    }

    public event Action OnWaterDrained;

    public void WaterDrained()
    {
        OnWaterDrained?.Invoke();
    }
}