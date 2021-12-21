using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextEventObject : TextEventObject_Base
{
    private GameManager gameManager = null;
    private Texts textsScript = null;

    [SerializeField]
    private List<SEventObjData> eventDatas = new List<SEventObjData>();
    [SerializeField]
    private float fadeOutTime = 3f;
    private float fadeOutTimer = 0f;

    private Animator anim = null;
    private SpriteRenderer spriteRenderer = null;

    private int eventNum = 0;
    private Vector2 originPos = Vector2.zero;

    [SerializeField]
    private bool isStartAtPlayer = false;
    public bool IsStartAtPlayer
    {
        get { return isStartAtPlayer; }
    }

    private bool textEventPlayed = false;
    private bool canDoEvent = true;
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
            else
            {
                textEventPlayed = value;
            }

            canDoEvent = value;
        }
    }

    private bool fadeOutStarted = false;

    private void Awake()
    {
        gameManager = GameManager.Instance;

        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        PosSet();
    }

    private void OnEnable()
    {
        gameManager.StageEnd += (e) =>
        {
            gameObject.SetActive(false);
        };

        textsScript = FindObjectOfType<Texts>(true);
        PosSet();

        spriteRenderer.color = new Vector4(1f, 1f, 1f, 1f);

        eventNum = 0;
    }
    private void OnDisable()
    {
        gameManager.StageEnd -= (e) =>
        {
            gameObject.SetActive(false);
        };
    }
    private void PosSet()
    {
        if (isStartAtPlayer)
        {
            transform.position = GameManager.Instance.player.transform.position;
        }

        originPos = transform.position;
    }
    private void Update()
    {
        if (canDoEvent)
        {
            DoMoveEvent();

            if (eventDatas[eventNum].doThisTextEvents.Count > 0 && !textEventPlayed)
            {
                eventDatas[eventNum].doThisTextEvents.ForEach(item =>
                {
                    ITextEvent temp = item.GetComponent<ITextEvent>();

                    if (temp == null)
                    {
                        Debug.LogWarning("The TextEventObj of " + gameObject.name + " that name is " + item.name + " has no ITextEvent Interface. This Obj had Destroyed at list, Fix it later.");
                        eventDatas[eventNum].doThisTextEvents.Remove(item);
                    }
                    else
                    {
                        temp.DoEvent();
                    }
                });

                textEventPlayed = true;
            }
        }

        FadeOut();
    }

    private void DoMoveEvent()
    {
        eventNum = Mathf.Clamp(eventNum, 0, eventDatas.Count - 1);

        if (isStartAtPlayer)
        {
            spriteRenderer.flipX = GameManager.Instance.player.spriteRenderer.flipX;
        }
        else
        {
            spriteRenderer.flipX = eventDatas[eventNum].flipX;
        }

        transform.position = Vector2.MoveTowards(transform.position, originPos + eventDatas[eventNum].moveTargetPos, eventDatas[eventNum].moveSpeed * Time.deltaTime);

        if (eventDatas[eventNum].animName != "")
        {
            spriteRenderer.color = new Vector4(1f, 1f, 1f, 1f);
            anim.Play(eventDatas[eventNum].animName);
        }
        else
        {
            spriteRenderer.color = new Vector4(1f, 1f, 1f, 0f);
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
        textEventPlayed = false;

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
