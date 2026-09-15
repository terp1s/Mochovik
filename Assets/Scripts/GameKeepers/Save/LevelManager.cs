using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public LevelDatabase levelDatabase;
    public static LevelManager Instance;
    public Canvas canvas;
    public AudioClip win;

    private void Awake()
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
    }
    public void LoadDatabase(LevelDatabase dat)
    {
        levelDatabase = dat;
        CompleteDatabase(levelDatabase.lvls.Count);
    }
    public void LoadGame(int level)
    {
        StartCoroutine(LoadNewLevel(level));
    }
    public void LoadLevelData(int index)
    {
        XMLSave currSave = levelDatabase.lvls[index - 1];

        LevelDataProcessor.Instance.LoadData(currSave);
    }
    public void RestartLevel()
    {
        LoadGame(LevelDataProcessor.Instance.CurrentLevel());
    }
    public void NextLevel()
    {
        PlayerPrefs.SetInt("level" + PlayerPrefs.GetInt("activePlayer"), PlayerPrefs.GetInt("level" + PlayerPrefs.GetInt("activePlayer")) + 1);

        //SoundManager.instance.PlaySound(win);
        //SoundManager.instance.ToggleMusic();


        if (levelDatabase.lvls.Count == LevelDataProcessor.Instance.levelData.levelIndex)
        {
            StartCoroutine(GameEnd());
        }
        else
        {   
            StartCoroutine(Victory());
        }
    }
    IEnumerator LoadNewLevel(int index)
    {
        SceneManager.LoadScene("Level");

        for (int i = 0; i < 2; i++)
        {
            yield return null;
        }

        LoadLevelData(index);
        Time.timeScale = 1f;
        StopAllCoroutines();
    }
    IEnumerator Victory()
    {
        Time.timeScale = 0f;
        GameObject.Find("Canvas").transform.Find("Victory").gameObject.SetActive(true);

        for (int i = 0; i < 1; i++)
        {
            yield return new WaitForSecondsRealtime(3.5f);
        }

        StartCoroutine(LoadNewLevel(LevelDataProcessor.Instance.levelData.levelIndex + 1));
        Time.timeScale = 1f;
        GameObject.Find("Canvas").transform.Find("Victory").gameObject.SetActive(false);

        yield return null;
        
    }
    public bool CheckForNextLevel()
    {
        if (GameObject.FindGameObjectsWithTag("Ghost").Length == 0)
        {
            NextLevel();

            return true;
        }
        else
        {
            return false;
        }
    }
    public void CompleteDatabase(int datCount)
    {
        foreach (string s in levelDatabase.lvls[0].newUnlockedPlants)
        {
            levelDatabase.lvls[0].unlockedPlants.Add(s);
        }

        for (int i = 1; i < datCount; i++)
        {
            foreach (string s in levelDatabase.lvls[i - 1].unlockedPlants)
            {
                levelDatabase.lvls[i].unlockedPlants.Add(s);
            }

            foreach(string s in levelDatabase.lvls[i].newUnlockedPlants)
            {
                levelDatabase.lvls[i].unlockedPlants.Add(s);
            }
        }
    }
    IEnumerator GameEnd()
    {
        for (int i = 0; i < 2; i++)
        {
            yield return new WaitForSecondsRealtime(1f);
        }

        Time.timeScale = 0f;
        GameObject.Find("Canvas").transform.Find("GameEnd").gameObject.SetActive(true);
    }
}
