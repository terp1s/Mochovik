using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Save
{
    [SerializeField]
    public int lvl;
    [SerializeField]
    public string playerName;

    public Save()
    {
        lvl = 1;
        playerName = "";

    }
}
