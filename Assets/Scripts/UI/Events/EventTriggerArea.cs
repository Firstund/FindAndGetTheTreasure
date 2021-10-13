using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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
    // TODO: 스타맵에디터의 트리거와 비슷한 기능을 하는 트리거스크립트를 만들것, 어떤 조건들이 충족되어야 하는지를 체크하는 방식, 어떤 키를 입력해야한다면 어떤 키를 입력해야하는지 List<string> 
    // 형태로 받아와서 이벤트 조건을 적용, 실행시킬것
    // 이벤트 실행에 의한 동작도 포함

    [SerializeField]
    private Conditions conditions;

    private bool playerEnterThisArea = false;
    private bool playerStayThisArea = false;
    private bool getKey = false;

    private bool doEvent = false;
    private bool playerIn = false;

    private float playerStayTimer = 0f;
    void Start()
    {

    }
    void Update()
    {
        if (playerEnterThisArea || playerStayThisArea || getKey)
        {
            GetKeyDownCheck();
            PlayerStayTimerCheck();
            CheckDoEvent();

            if (doEvent)
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
        if (playerIn)
        {
            if (playerStayTimer < conditions.stayThisArea.staySec)
            {
                playerStayTimer += Time.deltaTime;

                playerStayThisArea = false;
            }
            else
            {
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
        Debug.Log("DoEvent!");
    }

    private void GetKeyDownCheck()
    {
        if (conditions.getKey.Length > 0)
        {
            getKey = true;

            foreach (var item in conditions.getKey)
            {
                if (!Input.GetKey(item))
                {
                    getKey = false;
                    break;
                }
            }
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
            if (1 << other.gameObject.layer == LayerMask.GetMask("PLAYER"))
            {
                playerEnterThisArea = true;
            }
        }
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (1 << other.gameObject.layer == LayerMask.GetMask("PLAYER"))
        {
            if (conditions.stayThisArea.stayThisArea)
            {
                playerIn = true;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (1 << other.gameObject.layer == LayerMask.GetMask("PLAYER"))
        {
            if (conditions.enterThisArea)
            {
                playerEnterThisArea = false;
            }

            playerIn = false;
            playerStayThisArea = false;
            playerStayTimer = 0f;
        }
    }
}
