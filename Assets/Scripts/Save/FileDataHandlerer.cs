using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System;

public class FileDataHandlerer
{
    private string directFilePath = "";

    private string dataFileName = "";

    public FileDataHandlerer(string dirpath, string datafilename)
    {
        this.directFilePath = dirpath;
        this.dataFileName = datafilename;
    }

    public Save Load()
    {
        string fullPath = Path.Combine(directFilePath, dataFileName);

        Save loadedSave = null;
        
        Debug.Log(fullPath);

        if (File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = "";

                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                loadedSave = JsonUtility.FromJson<Save>(dataToLoad);

            }
            catch (Exception e)
            {
                Debug.LogError("loading didnt work");
            }
        }

        return loadedSave;
    }

    public void Save(Save data)
    {
        string fullPath = Path.Combine(directFilePath, dataFileName);


        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            string dataToStore = JsonUtility.ToJson(data, true);

            using(FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using(StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }
        }
        catch(Exception e)
        {
            Debug.LogError("saving :((");
        }
    }
}
