using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;
using System;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
public class DialogueManager : MonoBehaviour
{
    [SerializeField] private TextAsset inkJson;
    private bool dialoguePlaying = false;
    private int currentChoiceIndex = -1;

    [SerializeField] private float skipCooldown = 0.5f;

    private bool isSubmitting = false;
    private float nextAllowedSubmitTime = 0f;

    private Story story;
    private void Awake()
    {
        story = new Story(inkJson.text);
    }
    private void OnEnable()
    {
        GameEventsManager.instance.dialogueEvents.onEnterDialogue += EnterDialogue;
        GameEventsManager.instance.inputEvents.onSubmitPressed += SubmitPressed;
        GameEventsManager.instance.dialogueEvents.onUpdateChoiceIndex += UpdateChoiceIndex;
    }
    private void OnDisable()
    {
        GameEventsManager.instance.dialogueEvents.onEnterDialogue -= EnterDialogue;
        GameEventsManager.instance.inputEvents.onSubmitPressed -= SubmitPressed;
        GameEventsManager.instance.dialogueEvents.onUpdateChoiceIndex -= UpdateChoiceIndex;
    }

    private async void EnterDialogue(string knotName)
    {
        if (dialoguePlaying)
        {
            return;
        }

        dialoguePlaying = true;
        GameEventsManager.instance.inputEvents.ChangeInputEventContext(InputEventContext.DIALOGUE);
        GameEventsManager.instance.dialogueEvents.DialogueStarted();


        if (!knotName.Equals(""))
        {
            story.ChoosePathString(knotName);
        }

        await ContinueOrExitStory();
    }
    
    private async Task ContinueOrExitStory()
    {
        if (story.currentChoices.Count > 0 && currentChoiceIndex != -1)
        {
            story.ChooseChoiceIndex(currentChoiceIndex);
            currentChoiceIndex = -1;
        }

        string dialogueLine = "";

        while (story.canContinue)
        {
            dialogueLine = story.Continue();

         
            foreach (string tag in story.currentTags)
            {
                await HandleTag(tag);
            }

            if (!IsLineBlank(dialogueLine))
            {
                break;
            }
        }

     
        if (!IsLineBlank(dialogueLine) || story.currentChoices.Count > 0)
        {
            GameEventsManager.instance.dialogueEvents.DisplayDialogue(dialogueLine, story.currentChoices);

            if (IsLineBlank(dialogueLine))
            {
                GameEventsManager.instance.dialogueEvents.TypingFinished();
            }
        }
        else
        {
            ExitDialogue();
        }
    }

    private async Task HandleTag(string tag)
    {
        string[] split = tag.Split(':');

        string key = split[0];
        string value = split.Length > 1 ? split[1] : "";

        switch (key)
        {
            case "speaker":
                GameEventsManager.instance.dialogueEvents.UpdateSpeaker(value);
                break;
            case "clear":
                ClearState();
                break;
            case "scene":
                BackgroundManager.instance.ChangeBackground(value);
                break;
            case "sfx":
                AudioManager.instance.PlaySFX(value);
                break;
            case "stop_sfx":
                AudioManager.instance.StopSFX(value);
                break;
            case "loop_sfx":
                AudioManager.instance.PlayLoopingSFX(value);
                break;
            case "wait":
                if (float.TryParse(value, out float seconds))
                {
                    await Task.Delay((int)(seconds * 1000));
                }
                break;
        }
    }
    private bool IsLineBlank(string dialogueLine)
    {
        return dialogueLine.Trim().Equals("") || dialogueLine.Trim().Equals("\n");
    }

    private void UpdateChoiceIndex(int choiceIndex)
    {
        this.currentChoiceIndex = choiceIndex;
    }

    private async void SubmitPressed(InputEventContext inputEventContext)
    {
        if (!inputEventContext.Equals(InputEventContext.DIALOGUE))
        {
            return;
        }

        if (isSubmitting || Time.time < nextAllowedSubmitTime)
        {
            return;
        }

        bool anySpeakerTyping = false;
        foreach (var speaker in FindObjectsByType<DialogueSpeakerText>(FindObjectsSortMode.None))
        {
            if (speaker.IsTyping)
            {
                anySpeakerTyping = true;
                break;
            }
        }

        if (anySpeakerTyping)
        {
            // If typing, tell the speakers to finish immediately
            GameEventsManager.instance.dialogueEvents.RequestFinishTyping();
            nextAllowedSubmitTime = Time.time + skipCooldown;

        }
        else
        {
            // If not typing, go to the next line
            isSubmitting = true;
            try
            {
                await ContinueOrExitStory();
            }
            finally
            {
                isSubmitting = false;
                nextAllowedSubmitTime = Time.time + skipCooldown;
            }
        }
    }

    private void ClearState()
    {
        foreach (var speaker in FindObjectsByType<DialogueSpeakerText>(FindObjectsSortMode.None))
        {
            speaker.ClearPanel();
        }

        foreach(var button in FindObjectsByType<DialogueChoiceButton>(FindObjectsSortMode.None))
        {
             button.ClearButton();
        }
    }

    private void ExitDialogue()
    {
        GameEventsManager.instance.inputEvents.ChangeInputEventContext(InputEventContext.DEFAULT);
        dialoguePlaying = false;
        SceneManager.LoadScene("Snail");
    }
}
