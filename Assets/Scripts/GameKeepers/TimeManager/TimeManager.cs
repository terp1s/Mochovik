using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance;

    private List<TimeEvent> events = new();

    private float currentTime;

    void Awake()
    {
        Instance = this;
    }
    
    void Update()
    {
        currentTime += Time.deltaTime;

        for (int i = events.Count - 1; i >= 0; i--)
        {
            if (events[i].cancelled)
            {
                events.RemoveAt(i);
                continue;
            }

            if (currentTime >= events[i].executeTime)
            {
                events[i].callback.Invoke();
                events.RemoveAt(i);
            }
        }

        if (events.Count == 0)
        {
            enabled = false;
        }
    }

    public TimeEvent Schedule(float delay, Action action)
    {
        TimeEvent newEvent = new TimeEvent(
            currentTime + delay,
            action
        );

        events.Add(newEvent);

        enabled = true;

        return newEvent;
    }
}
   
