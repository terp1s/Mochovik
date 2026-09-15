using System.Collections.Generic;

[System.Serializable]
public class LevelDatabase
{
    public List<XMLSave> lvls = new List<XMLSave>();

    public void Add(XMLSave save)
    {
        lvls.Add(save);
    }
}


