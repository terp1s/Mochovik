using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroDialogue : MonoBehaviour
{
    [SerializeField] private string dialogueKnotName = "intro";
    private void OnEnable()
    {
        GameEventsManager.instance.inputEvents.onSubmitPressed += SubmitPressed;
    }

    private void SubmitPressed(InputEventContext context)
    {
        if (!context.Equals(InputEventContext.DEFAULT))
        {
            return;
        }

        if (!dialogueKnotName.Equals(""))
        {
            GameEventsManager.instance.dialogueEvents.EnterDialogue(dialogueKnotName);
        }
    }
}
