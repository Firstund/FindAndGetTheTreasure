using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextEventObject : TextEventObject_Base
{
    private Texts textsScript = null;

    [SerializeField]
    private List<SEventObjData> eventDatas = new List<SEventObjData>();
    [SerializeField]
    private float fadeOutTime = 3f;
    private float fadeOutTimer = 0f;

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

    private bool fadeOutStarted = false;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        originPos = transform.position;
    }
    private void OnEnable()
    {
        textsScript = FindObjectOfType<Texts>(true);

        spriteRenderer.color = new Vector4(1f, 1f, 1f, 1f);

        eventNum = -1;
    }
    private void Update()
    {
        if (canDoEvent)
        {
            DoEvent();
        }

        FadeOut();
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
    public void StartFadeOut()
    {
        fadeOutTimer = fadeOutTime;
        fadeOutStarted = true;
    }
    private void FadeOut()
    {
        if (fadeOutTimer > 0f)
        {
            fadeOutTimer -= Time.deltaTime;

            spriteRenderer.color = new Vector4(1f, 1f, 1f, fadeOutTimer / fadeOutTime);
        }
        else if (fadeOutStarted)
        {
            fadeOutStarted = false;
            gameObject.SetActive(false);
        }
    }
}
