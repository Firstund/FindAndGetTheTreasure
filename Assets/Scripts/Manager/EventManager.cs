using System;
using System.Collections;
using System.Collections.Generic;

public class EventManager
{
    private static Dictionary<string, Action> eventDictionary = new Dictionary<string, Action>();
    public static void StartListening(string eventName, Action listener)
    {
        Action thisEvent;

        if(eventDictionary.TryGetValue(eventName, out thisEvent)) // 같은 이름의 DIctionary있는지 체크
        {
            thisEvent += listener;                   // 같은 이름있을 때 구독
            eventDictionary[eventName] = thisEvent;
        }
        else
        {
            eventDictionary.Add(eventName, listener);
        }
    }
    public static void StopListening(string eventName, Action listener)
    {
        Action thisEvent;

        if(eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent -= listener;
            eventDictionary[eventName] = thisEvent;
        }
        else
        {
            eventDictionary.Remove(eventName);
        }
    }
    public static void TriggerEvent(string eventName)
    {
        Action thisEvent;

        if(eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent?.Invoke();
        }
        
    }
}
