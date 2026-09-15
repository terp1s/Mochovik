using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;

public class SaveManager : MonoBehaviour
{
    [SerializeField]
    private string fileName;
    private FileDataHandlerer fileDataHandlerer;
    public static SaveManager Instance { get; private set; }
    private Save save;
    private List<IDataPersistance> dataPersistanceObjects;

    private void Awake()
    {
        fileName = "save.json";
        if (Instance != null)
        {
            Debug.LogError("uz manager existuje :((");
        }

        Instance = this;
    }
    private void Start()
    {
        this.fileDataHandlerer = new FileDataHandlerer(Application.persistentDataPath, fileName);
        this.dataPersistanceObjects = FindAllPersistanceObjects();
        LoadGame();
    }

    public void NewGame()
    {
        this.save = new Save();
    }

    public void LoadGame()
    {
        save = fileDataHandlerer.Load();
        Debug.Log("load game");
        if(this.save == null)
        {
            Debug.Log("zadna savenuta hra, jede se od znova juhu");
            NewGame();
        }
        
        foreach(IDataPersistance obj in dataPersistanceObjects)
        {
            obj.LoadSave(save);
        }
    }

    public void SaveGame()
    {
        foreach (IDataPersistance obj in dataPersistanceObjects)
        {
            obj.SaveSave(ref save);
        }

        fileDataHandlerer.Save(save);
    }
    private void OnApplicationQuit()
    {
        SaveGame();
    }
    private List<IDataPersistance> FindAllPersistanceObjects()
    {
        IEnumerable<IDataPersistance> persistanceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistance>();
        return new List<IDataPersistance>(persistanceObjects);
    }
}
