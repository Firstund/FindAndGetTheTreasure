using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer
{
    public float startTime = 0f;

    public float originTime = 0f;
    public float endTime = 0f;
    public float currentTime = 0f;

    public float speed = 0f;

    public bool timerStart = false;

    public Action callBack = () => {};

    public void StartTimer()
    {
        StartTimer(originTime, endTime, speed, callBack, startTime);
    }
    public void StartTimer(float _originTime, float _endTime, float _speed, Action _callBack, float _startTime = 0f)
    {
        timerStart = true;

        originTime = _originTime;
        endTime = _endTime;
        speed = _speed;

        startTime = _startTime;
        currentTime = _startTime;

        callBack += _callBack;
    }
    public void CheckTimer(float _delta)
    {
        if(timerStart)
        {
            currentTime += _delta * speed;

            Debug.Log(currentTime);

            if(currentTime >= endTime)
            {
                timerStart = false;
                currentTime = endTime;

                callBack?.Invoke();
            }
        }
    }
}
public class UtilTimer : MonoBehaviour
{
    private static UtilTimer instance = null;
    public static UtilTimer Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<UtilTimer>();

                if(instance == null)
                {
                    instance = new GameObject("UtilTimer").AddComponent<UtilTimer>();
                }
            }

            return instance;
        }
    }

    private Dictionary<object, Timer> timerDict = new Dictionary<object, Timer>();
    public Dictionary<object, Timer> TimerDict
    {
        get
        {
            return timerDict;
        }
    }

    private void Update() 
    {
        CheckTimerAll(Time.deltaTime);
    }

    public void AddTimerDict(object key, float originTime, float endTime, float speed, Action callBack, float startTime = 0f)
    {
        Timer timer = new Timer();

        timer.originTime = originTime;
        timer.endTime = endTime;
        timer.speed = speed;
        timer.callBack = callBack;
        timer.startTime = startTime;

        if(timerDict.ContainsKey(key))
        {
            if(timerDict[key].timerStart)
            {
                Debug.LogWarning("Timer의 Key값이 실행중인 Timer와 곂칩니다.");
            }
            else
            {
                timerDict[key] = timer;
            }
        }   
        else
        {
            timerDict.Add(key, timer);
        }
    }
    public void AddAndStartTimerDict(object key, float originTime, float endTime, float speed, Action callBack, float startTime = 0f)
    {
        Timer timer = new Timer();

        if(timerDict.ContainsKey(key))
        {
            if(timerDict[key].timerStart)
            {
                Debug.LogWarning("Timer의 Key값이 실행중인 Timer와 곂칩니다.");
            }
            else
            {
                timerDict[key] = timer;
            }
        }
        else
        {
            timerDict.Add(key, timer);
        }

        timerDict[key].StartTimer(originTime, endTime, speed, callBack, startTime);
    }
    public void StartTimerDict(object key)
    {
        if(timerDict.ContainsKey(key))
        {
            timerDict[key].StartTimer();
        }
    }
    public void CheckTimerAll(float delta)
    {
        foreach(var timer in timerDict)
        {
            if(timer.Value.timerStart)
            {
                timer.Value.CheckTimer(delta);
            }
        }
    }
}
