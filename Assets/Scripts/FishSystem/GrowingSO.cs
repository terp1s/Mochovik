using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]


[CreateAssetMenu(fileName = "GrowingSO", menuName = "Scriptable Objects/GrowingSO")]
public class GrowingSO : ScriptableObject
{
    public string Name;
    public List<FishItem> growthStages;
    public float growthInterval;
    public int MaxPhase => growthStages.Count;
}

public class ItemData
{
    public string name;
}