using UnityEngine;

public class SceneMusic : MonoBehaviour
{
    [Header("Scene Track")]
    [SerializeField] private string musicTrackName;
    [SerializeField] private float fadeDuration = 1.0f;

    private void Start()
    {
        if (AudioManager.instance != null && !string.IsNullOrEmpty(musicTrackName))
        {
            AudioManager.instance.PlayMusic(musicTrackName, fadeDuration);
        }
    }
}