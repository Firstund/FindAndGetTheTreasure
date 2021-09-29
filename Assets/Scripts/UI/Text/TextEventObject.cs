using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextEventObject : TextEventObject_Base
{
    private Texts textsScript = null;

    [SerializeField]
    private List<SEventObjData> eventDatas = new List<SEventObjData>();

    private Animator anim = null;
    private SpriteRenderer spriteRenderer = null;

    private int eventNum = -1;
    private Vector2 originPos = Vector2.zero;

    private bool canDoEvent = false;
    public bool CanDoEvent
    {
        get
        {
            return canDoEvent;
        }
        set
        {
            if (value)
            {
                eventNum++;
                originPos = transform.position;
            }

            canDoEvent = value;
        }
    }

    private void Start()
    {
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        originPos = transform.position;
    }
    private void OnEnable()
    {
        textsScript = FindObjectOfType<Texts>(true);

        eventNum = -1;
    }
    private void Update()
    {
        if (canDoEvent)
        {
            DoEvent();
        }
    }

    private void DoEvent()
    {
        spriteRenderer.flipX = eventDatas[eventNum].flipX;

        transform.position = Vector2.MoveTowards(transform.position, originPos + eventDatas[eventNum].moveTargetPos, eventDatas[eventNum].moveSpeed * Time.deltaTime);

        if (eventDatas[eventNum].animName != "")
        {
            anim.Play(eventDatas[eventNum].animName);
        }
        else
        {
            anim.SetTrigger("GoToFirst");
        }

        float distance = Vector2.Distance(transform.position, originPos + eventDatas[eventNum].moveTargetPos);

        if (distance <= 0.1f && eventDatas[eventNum].setCanNextTalkByMoves)
        {
            OnEventEnd();
        }
    }

    private void OnEventEnd()
    {
        canDoEvent = false;
        anim.SetTrigger("GoToFirst");
        textsScript.canNextTalk = eventDatas[eventNum].setCanNextAtThisEventEnds;
    }
}
