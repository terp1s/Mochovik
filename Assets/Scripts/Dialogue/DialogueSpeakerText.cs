using Ink.Runtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Rendering;
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

            typingCoroutine = StartCoroutine(TypeLine(currentLine));
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

        // Notify that we are done so choices can show up
        GameEventsManager.instance.dialogueEvents.TypingFinished();
    }

    IEnumerator TypeLine(string line)
    {
        isTyping = true;

        dialogueText.text = line;
        dialogueText.maxVisibleCharacters = 0;

        foreach (char c in line)
        {
            dialogueText.maxVisibleCharacters++;

            if (c == ' ')
                continue;

            float delay = letterDelay;

            switch (c)
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

        GameEventsManager.instance.dialogueEvents.TypingFinished();

        isTyping = false;
    }

    public void ResetPanel()
    {
        dialogueText.text = "";
        isCurrentSpeaker = false;
    }
}
