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

    [SerializeField]
    private bool triggerLooping = false;

    private bool playerEnterThisArea = false;
    private bool playerStayThisArea = false;
    private bool getKey = false;

    private bool doEvent = false;
    private bool playerIn = false;

    public bool enableOnStart { private get; set; }

    private bool playerStayTimerIsPlaying = false;
    private float playerStayTimer = 0f;

    private void Awake()
    {
        eventTriggerAreaList.ForEach(e => e.enableOnStart = true);
    }
    private void Start()
    {
        gameManager = GameManager.Instance;

        playerStayTimer = conditions.stayThisArea.staySec;

        enabled = !enableOnStart;
    }

    void Update()
    {
        if (conditions.enterThisArea || conditions.stayThisArea.stayThisArea || conditions.getKey.Length > 0)
        {
            GetKeyDownCheck();
            PlayerStayTimerCheck();
            CheckDoEvent();

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

            if (doEvent && !gameManager.stopTime)
            {
                DoEvent();
            }
        }
        else
        {
            Debug.LogError("This EventTrigger of 'GameObject: " + gameObject.name + "' can't do Event! Because It has no Conditions! Check It!");
            enabled = false;
        }
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
        doEvent = true;

        if ((conditions.enterThisArea && !playerEnterThisArea) || (conditions.stayThisArea.stayThisArea && !playerStayThisArea) || (conditions.getKey.Length > 0 && !getKey))
        {
            doEvent = false;
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

            eventTriggerAreaList.ForEach(e => e.enabled = true);

            if (!triggerLooping)
            {
                enabled = false;
            }
    }
    public void OnEndEvent()
    {
        eventTriggerAreaList.ForEach(e => e.enabled = true);

        if (!triggerLooping)
        {
            enabled = false;
        }
    }

    private void GetKeyDownCheck()
    {
        if (conditions.getKey.Length > 0)
        {
            getKey = true;

            conditions.getKey.ForEach(item =>
            {
                if (!Input.GetKey(item))
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
}
