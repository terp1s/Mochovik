using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeManager : MonoBehaviour
{
    public static HomeManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    public void NewGame()
    {
        Debug.Log("Starting a new game...");
        SceneManager.LoadScene("Intro");
    }

    public void LoadGame()
    {
        Debug.Log("Load Game functionality is not implemented yet.");
    }

    public void ExitGame()
    {
        Debug.Log("Exiting the game...");
        Application.Quit();
    }
}
