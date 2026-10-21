using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class DialogueAudioPlayer : MonoBehaviour
{
    public static DialogueAudioPlayer Instance { get; private set; }

    [Header("Audio Clips")]
    [SerializeField] private AudioClip[] dialogueBlips;

    [Header("Random Pitch Modulation")]
    [Range(0.5f, 1.5f)][SerializeField] private float minPitch = 0.85f;
    [Range(0.5f, 1.5f)][SerializeField] private float maxPitch = 1.15f;

    [Header("Frequency")]
    [SerializeField] private int characterFrequency = 1;

    private AudioSource audioSource;
    private int characterCounter = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    
    public void PlayCharacterSound(char c)
    {
        if (char.IsWhiteSpace(c)) return;

        characterCounter++;
        if (characterCounter % characterFrequency != 0) return;

        if (dialogueBlips == null || dialogueBlips.Length == 0) return;

        AudioClip clip = dialogueBlips[Random.Range(0, dialogueBlips.Length)];

        audioSource.pitch = Random.Range(minPitch, maxPitch);

        audioSource.PlayOneShot(clip);
    }

    public void ResetCounter()
    {
        characterCounter = 0;
    }

    public void StopAudio()
    {
        audioSource.Stop();
    }
}