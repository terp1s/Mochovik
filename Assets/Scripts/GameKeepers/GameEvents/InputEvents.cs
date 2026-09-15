using UnityEngine;
using System;

public class InputEvents
{
    public InputEventContext inputEventContext { get; private set; } = InputEventContext.DEFAULT;

    public void ChangeInputEventContext(InputEventContext newContext)
    {
        this.inputEventContext = newContext;
    }   

    public event Action<InputEventContext> onSubmitPressed;
    public void SubmitPressed()
    {
        if (onSubmitPressed != null)
        {
            onSubmitPressed(this.inputEventContext);
        }
    }

    public event Action onLeftMouseClicked;
    public void LeftMouseClicked()
    {
        if (onLeftMouseClicked != null)
        {
            onLeftMouseClicked();
        }
    }

    public event Action onRightMouseClicked;
    public void RightMouseClicked()
    {
        if (onRightMouseClicked != null)
        {
            onRightMouseClicked();
        }
    }
}