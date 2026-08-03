using System;
using UnityEngine;
using UnityEngine.UIElements;

public class GameEventsManager : MonoBehaviour
{
    public static GameEventsManager instance { get; private set; }


    public DialogueEvents dialogueEvents;
    public InputEvents inputEvents;
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one Game Events Manager in the scene.");
        }
        instance = this;

        // initialize all events
       
        dialogueEvents = new DialogueEvents();
        inputEvents = new InputEvents();
    }
}