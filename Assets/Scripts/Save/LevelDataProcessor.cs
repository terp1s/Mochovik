using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

public class LevelDataProcessor : MonoBehaviour
{
    public static LevelDataProcessor Instance;
    public LevelData levelData;
    public List<IXMLDataHandlerer> dataPersistanceObjects;
    public List<GameObject> plantsAndGhosts;
    public bool isPutEnabled;

    public void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(this);

            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        isPutEnabled = false;
    }
    public void LoadData(XMLSave save)
    {
        levelData = new LevelData();

        levelData.unlockedPlants = FindInGame(save.unlockedPlants);
        levelData.levelIndex = save.lvl;
        levelData.ghostCount = save.ghostCount;
        levelData.isDay = save.isDay;
        levelData.ghosts = FindInGame(save.Ghosts);

        dataPersistanceObjects = FindDataPersistanceObjects();

        foreach (IXMLDataHandlerer xMLDataHandlerer in dataPersistanceObjects)
        {
            xMLDataHandlerer.Load(levelData);
        }
    }
    public List<GameObject> FindInGame(List<string> list)
    {
        List<GameObject> result = new List<GameObject>();

        foreach(string str in list)
        {
            result.Add(plantsAndGhosts.Find(item => item.name == str));
        }

        return result;
    }
    public int CurrentLevel()
    {
        return levelData.levelIndex;
    }    
    public List<IXMLDataHandlerer> FindDataPersistanceObjects()
    {
        IEnumerable<IXMLDataHandlerer> dataPersistanceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IXMLDataHandlerer>();

        return new List<IXMLDataHandlerer>(dataPersistanceObjects);
    }
}
