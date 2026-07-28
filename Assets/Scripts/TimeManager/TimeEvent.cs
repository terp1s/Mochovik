using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TimeEvent
{
    public float executeTime;
    public Action callback;
    public bool cancelled;

    public TimeEvent(float time, Action action)
    {
        executeTime = time;
        callback = action;
        cancelled = false;
    }
}
