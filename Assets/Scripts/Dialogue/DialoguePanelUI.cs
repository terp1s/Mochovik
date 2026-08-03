using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;

public class DialoguePanelUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private GameObject contentParent;
    [SerializeField] private List<DialogueSpeakerText> dialogueSpeakerTexts;
    [SerializeField] private DialogueChoiceButton[] choiceButtons;
    private List<Choice> currentAvailableChoices;


    private void Awake()
    {
        contentParent.SetActive(false);
        ResetPanel();
    }

    private void OnEnable()
    {
        GameEventsManager.instance.dialogueEvents.onDialogueStarted += DialogueStarted;
        GameEventsManager.instance.dialogueEvents.onDialogueFinished += DialogueFinished;
        GameEventsManager.instance.dialogueEvents.onDisplayDialogue += DisplayDialogue;
        GameEventsManager.instance.dialogueEvents.onTypingFinished += ShowChoices;

    }

    private void OnDisable()
    {
        GameEventsManager.instance.dialogueEvents.onDialogueStarted -= DialogueStarted;
        GameEventsManager.instance.dialogueEvents.onDialogueFinished -= DialogueFinished;
        GameEventsManager.instance.dialogueEvents.onDisplayDialogue -= DisplayDialogue;
        GameEventsManager.instance.dialogueEvents.onTypingFinished -= ShowChoices;

    }

    private void DialogueStarted()
    {
        contentParent.SetActive(true);
    }

    private void DialogueFinished()
    {
        contentParent.SetActive(false);

        // reset anything for next time
        ResetPanel();
    }

    private void DisplayDialogue(string dialogueLine, List<Choice> dialogueChoices)
    {
        foreach (DialogueChoiceButton choiceButton in choiceButtons)
        {
            choiceButton.gameObject.SetActive(false);
        }

        // Store choices to show later
        currentAvailableChoices = dialogueChoices;

        
        // defensive check - if there are more choices coming in than we can support, log an error
        if (dialogueChoices.Count > choiceButtons.Length)
        {
            Debug.LogError("More dialogue choices ("
                + dialogueChoices.Count + ") came through than are supported ("
                + choiceButtons.Length + ").");
        }
    }

    private void ShowChoices()
    {
        if (currentAvailableChoices == null || currentAvailableChoices.Count == 0) return;

        // Enable and set info for buttons
        int choiceButtonIndex = currentAvailableChoices.Count - 1;
        for (int inkChoiceIndex = 0; inkChoiceIndex < currentAvailableChoices.Count; inkChoiceIndex++)
        {
            Choice dialogueChoice = currentAvailableChoices[inkChoiceIndex];
            DialogueChoiceButton choiceButton = choiceButtons[choiceButtonIndex];

            choiceButton.gameObject.SetActive(true);
            choiceButton.SetChoiceText(dialogueChoice.text);
            choiceButton.SetChoiceIndex(inkChoiceIndex);

            if (inkChoiceIndex == 0)
            {
                choiceButton.SelectButton();
                GameEventsManager.instance.dialogueEvents.UpdateChoiceIndex(inkChoiceIndex);
            }

            choiceButtonIndex--;
        }

        // Clear storage so they don't show twice
        currentAvailableChoices = null;
    }

    private void ResetPanel()
    {
        foreach (var panel in dialogueSpeakerTexts)
        {
            panel.ResetPanel();
        }
    }
}