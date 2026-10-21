using Ink.Runtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueSpeakerText : MonoBehaviour
{
    public string speakerName;
    [SerializeField] private bool isCurrentSpeaker = false;
    [SerializeField] private TextMeshProUGUI dialogueText;
    private bool isTyping = false;
    public bool IsTyping => isTyping;

    private Coroutine typingCoroutine;
    private string currentLine;


    [SerializeField] private float letterDelay = 0.04f;
    [SerializeField] private float commaDelay = 0.20f;
    [SerializeField] private float sentenceDelay = 0.35f;
    [SerializeField] private float newLineDelay = 0.5f;

    private void OnEnable()
    {
        GameEventsManager.instance.dialogueEvents.onUpdateSpeaker += UpdateSpeaker;
        GameEventsManager.instance.dialogueEvents.onDisplayDialogue += DisplayDialogue;
        GameEventsManager.instance.dialogueEvents.onFinishTypingRequested += FinishTyping;

    }

    private void OnDisable()
    {
        GameEventsManager.instance.dialogueEvents.onUpdateSpeaker -= UpdateSpeaker;
        GameEventsManager.instance.dialogueEvents.onDisplayDialogue -= DisplayDialogue;
        GameEventsManager.instance.dialogueEvents.onFinishTypingRequested -= FinishTyping;

    }

    private void UpdateSpeaker(string name)
    {
        if (name.Equals(speakerName))
        {
            isCurrentSpeaker = true;
        }
        else
        {
            ResetPanel();
        }
    }

    private void DisplayDialogue(string line, List<Choice> dialogueChoices)
    {
        if (isCurrentSpeaker)
        {
            currentLine = line;

            if (typingCoroutine != null)
                StopCoroutine(typingCoroutine);

            typingCoroutine = StartCoroutine(TypewriterRoutine(currentLine));
            GameEventsManager.instance.dialogueEvents.TypingStarted();
        }
    }
    void FinishTyping()
    {
        if (!isTyping || !isCurrentSpeaker) return;

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        dialogueText.maxVisibleCharacters = currentLine.Length;
        isTyping = false;

        if (DialogueAudioPlayer.Instance != null)
            DialogueAudioPlayer.Instance.StopAudio();

        GameEventsManager.instance.dialogueEvents.TypingFinished();
    }
    private IEnumerator TypewriterRoutine(string fullText)
    {
        isTyping = true;

        //fullText = fullText.TrimEnd('\r', '\n');

        dialogueText.text = fullText;
        dialogueText.maxVisibleCharacters = 0;



        if (DialogueAudioPlayer.Instance != null)
            DialogueAudioPlayer.Instance.ResetCounter();

        for (int i = 0; i < fullText.Length; i++)
        {
            char currentChar = fullText[i];

            dialogueText.maxVisibleCharacters++;


            if (DialogueAudioPlayer.Instance != null)
                DialogueAudioPlayer.Instance.PlayCharacterSound(currentChar);


            if (currentChar == ' ')
            {
                continue;
            }

            if (i >= fullText.Length - 1) break;

            float delay = letterDelay;

            switch (currentChar)
            {
                case ',':
                    delay = commaDelay;
                    break;
                case '.':
                case '!':
                case '?':
                    delay = sentenceDelay;
                    break;
                case '\n':
                    delay = newLineDelay;
                    break;
            }
            yield return new WaitForSeconds(delay);
        }

        if (DialogueAudioPlayer.Instance != null)
            DialogueAudioPlayer.Instance.StopAudio();

        isTyping = false;
        GameEventsManager.instance.dialogueEvents.TypingFinished();
    }

    public void ClearPanel()
    {
        dialogueText.text = "";

    }
    public void ResetPanel()
    {
        ClearPanel();
        isCurrentSpeaker = false;
    }
}
