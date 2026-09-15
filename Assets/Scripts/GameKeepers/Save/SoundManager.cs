using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public Slider slider;
    public AudioSource music, effects;
    public static SoundManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    void Start()
    {
        if (PlayerPrefs.HasKey("activePlayer"))
        {
            Load();
        }
        else
        {
            slider.value = 1;
        }
    }
    public void PlaySound(AudioClip clip)
    {
        effects.PlayOneShot(clip);
    }
    public void ToggleMusic()
    {
        music.mute = !music.mute;
    }
    public void ToggleEffect()
    {
        effects.mute = !effects.mute;
    }
    public void VolumeChange()
    {
        AudioListener.volume = slider.value;
        Save();
    }
    public void Save()
    {
        PlayerPrefs.SetFloat("volume" + PlayerPrefs.GetInt("activePlayer"), slider.value);
    }
    public void Load()
    {
        slider.value = PlayerPrefs.GetFloat("volume" + PlayerPrefs.GetInt("activePlayer"));
    }
}
