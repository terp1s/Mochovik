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

    [Header("Music Library")]
    public List<Sound> music;
    private Dictionary<string, Sound> musicDictionary;

    private Coroutine musicFadeRoutine;
    private string currentMusicName = "";


    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        //DontDestroyOnLoad(gameObject);

        soundDictionary = new Dictionary<string, Sound>();
        foreach (Sound s in sounds)
        {
            soundDictionary[s.name] = s;
        }

        musicDictionary = new Dictionary<string, Sound>();
        foreach (Sound m in music)
        {
            musicDictionary[m.name] = m;
        }
    }

    public void PlayMusic(string musicName, float fadeDuration = 1f)
    {
        if (currentMusicName == musicName && musicSource.isPlaying)
            return;

        if (!musicDictionary.ContainsKey(musicName))
        {
            Debug.LogWarning($"Music: '{musicName}' not found in AudioManager!");
            return;
        }

        Sound newMusic = musicDictionary[musicName];
        currentMusicName = musicName;

        if (musicFadeRoutine != null)
            StopCoroutine(musicFadeRoutine);

        musicFadeRoutine = StartCoroutine(FadeMusicRoutine(newMusic, fadeDuration));
    }

    private IEnumerator FadeMusicRoutine(Sound newMusic, float duration)
    {
        float startVolume = musicSource.volume;

        if (musicSource.isPlaying && duration > 0f)
        {
            for (float t = 0; t < duration * 0.5f; t += Time.deltaTime)
            {
                musicSource.volume = Mathf.Lerp(startVolume, 0f, t / (duration * 0.5f));
                yield return null;
            }
        }

        musicSource.clip = newMusic.clip;
        musicSource.pitch = newMusic.pitch;
        musicSource.loop = true;
        musicSource.Play();

        float targetVolume = newMusic.volume;
        if (duration > 0f)
        {
            for (float t = 0; t < duration * 0.5f; t += Time.deltaTime)
            {
                musicSource.volume = Mathf.Lerp(0f, targetVolume, t / (duration * 0.5f));
                yield return null;
            }
        }

        musicSource.volume = targetVolume;
    }

    public void StopMusic(float fadeDuration = 1f)
    {
        currentMusicName = "";
        if (musicFadeRoutine != null) StopCoroutine(musicFadeRoutine);
        musicFadeRoutine = StartCoroutine(FadeOutMusicRoutine(fadeDuration));
    }

    private IEnumerator FadeOutMusicRoutine(float duration)
    {
        float startVolume = musicSource.volume;
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            musicSource.volume = Mathf.Lerp(startVolume, 0f, t / duration);
            yield return null;
        }
        musicSource.Stop();
    }

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
            if (loopingSources.ContainsKey(soundName)) return;

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
            Destroy(loopingSources[soundName]); 
            loopingSources.Remove(soundName);
        }
    }

    /*
    public void PlayMusic(string musicName)
    {
        if (musicDictionary.ContainsKey(musicName))
        {
            Sound s = musicDictionary[musicName];
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
    */
}