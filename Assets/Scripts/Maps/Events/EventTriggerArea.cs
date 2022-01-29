using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

using UnityEngine;

[Serializable]
public struct Conditions_StayThisArea
{
    public bool stayThisArea;
    public float staySec;
}
[Serializable]
public struct Conditions
{
    public bool enterThisArea;
    public Conditions_StayThisArea stayThisArea;
    public KeyCode[] getKey;
}
public class EventTriggerArea : MonoBehaviour
{
    private GameManager gameManager = null;

    [SerializeField]
    private Conditions conditions;
    [SerializeField]
    private GameObject eventTriggerObj = null;
    [SerializeField]
    private LayerMask targetObjLayer;

    [Header("이 이벤트가 활성화 되고 나서 활성화 될 EventTriggerArea들")]
    [SerializeField]
    private List<EventTriggerArea> eventTriggerAreaList = new List<EventTriggerArea>();

    private bool eventTriggerAreaEnabled = false;

    [Header("플레이어가 지상에 있을 때 eventTriggerArea들을 활성화 한다.")]
    [SerializeField]
    private bool enableTrueEventTriggerAreasWhenIsGround = false;
    private bool playerIsGround = false;

    [SerializeField]
    private bool triggerLooping = false;
    [SerializeField]
    private bool doItWhenStopTime = false;

    private bool playerEnterThisArea = false;
    private bool playerStayThisArea = false;
    private bool getKey = false;

    private bool doEvent = false;
    private bool eventPlayed = false;
    private bool playerIn = false;

    public bool disableOnStart { private get; set; }

    private bool playerStayTimerIsPlaying = false;
    private float playerStayTimer = 0f;


    private void Awake()
    {
        gameManager = GameManager.Instance;

        eventTriggerAreaList.ForEach(e => e.disableOnStart = true);
    }
    private void OnEnable()
    {
        if (enableTrueEventTriggerAreasWhenIsGround)
        {
            EventManager.StartListening("WhenPlayerInAirToGround", WhenIsAirToGround);
        }
    }
    private void Start()
    {
        playerStayTimer = conditions.stayThisArea.staySec;

        enabled = !disableOnStart;
    }
    private void OnDisable()
    {
        if (enableTrueEventTriggerAreasWhenIsGround)
        {
            EventManager.StopListening("WhenPlayerInAirToGround", WhenIsAirToGround);
        }
    }

    private void WhenIsAirToGround()
    {
        if (eventPlayed)
        {
            playerIsGround = true;
        }
    }

    void Update()
    {
        if (CheckConditions())
        {
            GetKeyDownCheck();
            PlayerStayTimerCheck();

            if (conditions.getKey.Length > 0 && !playerStayTimerIsPlaying)
            {
                if (getKey)
                {
                    playerStayTimer = 0f;
                }
            }
            else if (!playerStayTimerIsPlaying)
            {
                playerStayTimer = 0f;
            }

            CheckDoEvent();

            if (doEvent)
            {
                if (doItWhenStopTime)
                {
                    if (triggerLooping)
                    {
                        DoEvent();
                    }
                    else if (!eventPlayed)
                    {
                        DoEvent();
                    }
                }
                else if (!gameManager.stopTime)
                {
                    if (triggerLooping)
                    {
                        DoEvent();
                    }
                    else if (!eventPlayed)
                    {
                        DoEvent();
                    }
                }
            }

            if (eventPlayed)
            {
                EventTriggerAreasEnabledTrue();
            }

            EnabledFalse();
        }
        else
        {
            Debug.LogError("This EventTrigger of 'GameObject: " + gameObject.name + "' can't do Event! Because It has no Conditions! Check It!");
            enabled = false;
        }
    }
    private bool CheckConditions()
    {
        return conditions.enterThisArea || conditions.stayThisArea.stayThisArea || conditions.getKey.Length > 0;
    }

    private void PlayerStayTimerCheck()
    {
        if (playerIn && !gameManager.stopTime)
        {
            if (playerStayTimer < conditions.stayThisArea.staySec)
            {
                playerStayTimer += Time.deltaTime;

                playerStayTimerIsPlaying = true;
                playerStayThisArea = false;
            }
            else
            {
                playerStayTimerIsPlaying = false;
                playerStayThisArea = true;
            }
        }
    }

    private void CheckDoEvent()
    {
        if ((conditions.enterThisArea && !playerEnterThisArea) || (conditions.stayThisArea.stayThisArea && !playerStayThisArea) || (conditions.getKey.Length > 0 && !getKey))
        {
            if (doEvent == true)
            {
                DoEventWhenDoEventFalse();
            }

            doEvent = false;
        }
        else
        {
            doEvent = true;
        }
    }

    private void DoEvent()
    {
        if (eventTriggerObj != null)
        {
            IEventTrigger eventTrigger = eventTriggerObj.GetComponent<IEventTrigger>();

            if (eventTrigger == null)
            {
                Debug.LogError(eventTriggerObj + " has no IEventTrigger!");
                return;
            }

            eventTrigger.DoEvent();
        }

        eventPlayed = true;

        if (gameManager.player.IsGround)
        {
            playerIsGround = true;
        }
    }
    private void DoEventWhenDoEventFalse() // doEvent변수가 true였다가 false로 바뀔 때 실행
    {
        if (eventTriggerObj != null)
        {
            IEventTrigger eventTrigger = eventTriggerObj.GetComponent<IEventTrigger>();

            if (eventTrigger == null)
            {
                Debug.LogError(eventTriggerObj + " has no IEventTrigger!");
                return;
            }

            eventTrigger.DoEventWhenDoEventFalse();
        }
    }
    private void EnabledFalse()
    {
        if (eventPlayed && !triggerLooping)
        {
            if (enableTrueEventTriggerAreasWhenIsGround)
            {
                if (eventTriggerAreaEnabled)
                {
                    enabled = false;
                }
            }
            else
            {
                enabled = false;
            }
        }
    }
    public void EventTriggerAreasEnabledTrue()
    {
        if (enableTrueEventTriggerAreasWhenIsGround)
        {
            if (playerIsGround)
            {
                eventTriggerAreaList.ForEach(e => e.enabled = true);
                eventTriggerAreaEnabled = true;
            }
        }
        else
        {
            eventTriggerAreaList.ForEach(e => e.enabled = true);
            eventTriggerAreaEnabled = true;
        }
    }

    private void GetKeyDownCheck()
    {
        if (conditions.getKey.Length > 0)
        {
            getKey = true;

            conditions.getKey.ForEach(item =>
            {
                if (!Input.GetKeyUp(item))
                {
                    getKey = false;
                }
            });
        }
        else
        {
            getKey = false;
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (conditions.enterThisArea)
        {
            if (1 << other.gameObject.layer == targetObjLayer)
            {
                playerEnterThisArea = true;
            }
        }
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (1 << other.gameObject.layer == targetObjLayer)
        {
            if (conditions.stayThisArea.stayThisArea)
            {
                playerIn = true;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (1 << other.gameObject.layer == targetObjLayer)
        {
            if (conditions.enterThisArea)
            {
                playerEnterThisArea = false;
            }

            playerIn = false;
            playerStayThisArea = false;
            playerStayTimerIsPlaying = false;
            playerStayTimer = 0f;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position, 0.35f);
    }
}
