using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Linq;
using UnityEngine.UIElements;
using System.IO.Pipes;

public class XMLManager : MonoBehaviour
{
    public static XMLManager instance;
    public LevelDatabase levelDatabase;
    public TextAsset leveldata;
    private void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(this);
            levelDatabase = Load();
            LevelManager.Instance.LoadDatabase(levelDatabase);

            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }  
    }
    public LevelDatabase Load()
    {

        XmlReader.Create(new MemoryStream(leveldata.bytes));

        XmlSerializer serializer = new XmlSerializer(typeof(LevelDatabase));
       

        if (leveldata != null)
        {
            using (XmlReader reader = XmlReader.Create(new StringReader(leveldata.text)))
            {
                while (reader.Read())
                {
                    levelDatabase = serializer.Deserialize(reader) as LevelDatabase;
                }
            }
        }
        else
        {
            Debug.LogError("XML file is not assigned.");
        }

        return levelDatabase;
    }
}


