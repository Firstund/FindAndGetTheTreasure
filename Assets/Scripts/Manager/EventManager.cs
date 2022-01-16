using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager
{
    private static Dictionary<string, Action> eventDictionary = new Dictionary<string, Action>();
    private static Dictionary<string, Action<Vector2>> vec2_EventDictionary = new Dictionary<string, Action<Vector2>>();

    public static void StartListening(string eventName, Action listener)
    {
        Action thisEvent;

        if (eventDictionary.TryGetValue(eventName, out thisEvent)) // 같은 이름의 DIctionary있는지 체크
        {
            thisEvent += listener;                   // 같은 이름있을 때 구독
            eventDictionary[eventName] = thisEvent;
        }
        else
        {
            eventDictionary.Add(eventName, listener);
        }
    }
    public static void StartListening(string eventName, Action<Vector2> listener)
    {
        Action<Vector2> thisEvent;

        if (vec2_EventDictionary.TryGetValue(eventName, out thisEvent)) // 같은 이름의 DIctionary있는지 체크
        {
            thisEvent += listener;                   // 같은 이름있을 때 구독
            vec2_EventDictionary[eventName] = thisEvent;
        }
        else
        {
            vec2_EventDictionary.Add(eventName, listener);
        }
    }

    public static void StopListening(string eventName, Action listener)
    {
        Action thisEvent;

        if (eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent -= listener;
            eventDictionary[eventName] = thisEvent;
        }
        else
        {
            eventDictionary.Remove(eventName);
        }
    }
    public static void StopListening(string eventName, Action<Vector2> listener)
    {
        Action<Vector2> thisEvent;

        if (vec2_EventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent -= listener;
            vec2_EventDictionary[eventName] = thisEvent;
        }
        else
        {
            vec2_EventDictionary.Remove(eventName);
        }
    }
    public static void TriggerEvent(string eventName)
    {
        Action thisEvent;

        if (eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent?.Invoke();
        }

    }
    public static void TriggerEvent(string eventName, Vector2 param)
    {
        Action<Vector2> thisEvent;

        if(vec2_EventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent?.Invoke(param);
        }
        
    }
}
