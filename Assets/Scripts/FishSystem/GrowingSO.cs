using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct FishVariation
{
    public Sprite sprite;
    public FishItem itemData;
}

[CreateAssetMenu(fileName = "GrowingSO", menuName = "Scriptable Objects/GrowingSO")]
public class GrowingSO : ScriptableObject
{
    public string Name;
    public List<FishVariation> growthStages;
    public float growthInterval;
    public int MaxPhase => growthStages.Count;
}

public class ItemData
{
    public string name;
}