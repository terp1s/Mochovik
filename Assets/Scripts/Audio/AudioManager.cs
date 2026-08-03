using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Sound Library")]
    public List<Sound> sounds;
    private Dictionary<string, Sound> soundDictionary;
    private Dictionary<string, AudioSource> loopingSources = new Dictionary<string, AudioSource>();


    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

        // Initialize Dictionary for fast lookup
        soundDictionary = new Dictionary<string, Sound>();
        foreach (Sound s in sounds)
        {
            soundDictionary[s.name] = s;
        }
    }

    // Play a sound once (for UI, hits, dialogue triggers)
    public void PlaySFX(string soundName)
    {
        if (soundDictionary.ContainsKey(soundName))
        {
            Sound s = soundDictionary[soundName];
            sfxSource.pitch = s.pitch;
            sfxSource.PlayOneShot(s.clip, s.volume);
        }
        else
        {
            Debug.LogWarning("Sound: " + soundName + " not found!");
        }
    }

    public void PlayLoopingSFX(string soundName)
    {
        if (soundDictionary.TryGetValue(soundName, out Sound s))
        {
            // If it's already playing, don't start it again
            if (loopingSources.ContainsKey(soundName)) return;

            // Create a temporary AudioSource for this specific loop
            AudioSource newSource = gameObject.AddComponent<AudioSource>();
            newSource.clip = s.clip;
            newSource.volume = s.volume;
            newSource.pitch = s.pitch;
            newSource.loop = true;
            newSource.Play();

            loopingSources.Add(soundName, newSource);
        }
    }

    public void StopSFX(string soundName)
    {
        if (loopingSources.ContainsKey(soundName))
        {
            loopingSources[soundName].Stop();
            Destroy(loopingSources[soundName]); // Clean up the component
            loopingSources.Remove(soundName);
        }
    }

    // Play looping music with a simple fade (optional)
    public void PlayMusic(string soundName)
    {
        if (soundDictionary.ContainsKey(soundName))
        {
            Sound s = soundDictionary[soundName];
            musicSource.clip = s.clip;
            musicSource.volume = s.volume;
            musicSource.loop = true;
            musicSource.Play();
        }
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }
}