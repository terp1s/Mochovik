using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BackgroundManager : MonoBehaviour
{
    public static BackgroundManager instance;

    [SerializeField] private List<Sprite> bgrList;
    private Dictionary<string, Sprite> bgrDictionary;


    [SerializeField] UnityEngine.UI.Image img;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        
        bgrDictionary = new Dictionary<string, Sprite>();
        foreach (Sprite t in bgrList)
        {
            bgrDictionary[t.name] = t;
        }
    }

    public void ChangeBackground(string imageName)
    {
        if (bgrDictionary.ContainsKey(imageName))
        {
            img.sprite = bgrDictionary[imageName];
        }
        else
        {
            Debug.LogWarning("Image: " + imageName + " not found!");
        }
    }
}
