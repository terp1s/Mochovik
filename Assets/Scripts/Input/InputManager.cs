using UnityEngine;
using UnityEngine.InputSystem;

// This script acts as a proxy for the PlayerInput component
// such that the input events the game needs to proces will 
// be sent through the GameEventManager. This lets any other
// script in the project easily subscribe to an input action
// without having to deal with the PlayerInput component directly.

public class InputManager : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Debug.Log("enter pressed");
            GameEventsManager.instance.inputEvents.SubmitPressed();
        }

        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("left mouse clicked");
            GameEventsManager.instance.inputEvents.LeftMouseClicked();
        }

        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("right mouse clicked");
            GameEventsManager.instance.inputEvents.RightMouseClicked();
        }
    }
}