using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using DG.Tweening;


public class Texts : Text_Base
{
    private GameManager gameManager = null;
    private StageManager stagemanager = null;
    private TalkManager talkManager = null;

    [Serializable]
    private struct SEventObjSpawnData
    {
        public GameObject eventObject;
        public Vector2 eventObjSpawnPos;
    }

    [Header("해당 대화 이벤트에서 사용할 EventObject들, 첫번째는 플레이어의 EventObject")]
    [SerializeField]
    private List<SEventObjSpawnData> eventObjSpawnData = new List<SEventObjSpawnData>();

    [SerializeField]
    private List<SText> texts = new List<SText>();
    [SerializeField]
    private Text text = null;

    [SerializeField]
    private Image LSpriteRenderer = null;
    [SerializeField]
    private Image RSpriteRenderer = null;

    [SerializeField]
    private GameObject nextButtonObj = null;
    private Text nextButtonTxt = null;
    [SerializeField]
    private GameObject currentTalkableObj = null;

    [SerializeField]
    private int currentTextNum = 0;

    private float doTextSpeed = 100f; // 초당 출력하는 글자의 수
    private float doTextEndTimer = 0f;

    [Header("이 값이 true면 TextEvent가 진행될 때 GameManager의 Player Object를 Despawn한다.")]
    [SerializeField]
    private bool despawnPlayerObjWhenTextEvent = false;
    [Header("이 값이 true면 TextEvent가 진행될 때 PlayerRespwnPosition을 이곳으로 바꾼다.")]
    [SerializeField]
    private bool SetPlayerRespawnPosition = true;

    [Header("이 값이 true면 대화가 끝났을 때 게임종료처리")]
    [SerializeField]
    private bool endGameAtEndTalk = false;
    [Header("이 값이 true면 게임종료시 게임클리어처리 false면 게임오버처리")]
    [SerializeField]
    private bool gameClearAtEndGame = false;
    private bool doFirstText = true;
    private bool doTextEnd = false;
    public bool canNextTalk = true;

    void Awake()
    {
        gameManager = GameManager.Instance;
        stagemanager = StageManager.Instance;
        talkManager = TalkManager.Instance;

        nextButtonTxt = nextButtonObj.GetComponentInChildren<Text>();
    }
    public void Update()
    {
        DoTextEndCheck();

        if (doFirstText)
        {
            SetText();

            if (eventObjSpawnData.Count > 0)
            {
                eventObjSpawnData[0].eventObject.transform.position = new Vector2(eventObjSpawnData[0].eventObject.transform.position.x, gameManager.player.currentPosition.y);
            }

            if (despawnPlayerObjWhenTextEvent)
            {
                gameManager.player.gameObject.SetActive(false);
            }
            else
            {
                gameManager.player.Rigid.velocity = Vector2.zero;
            }

            doFirstText = false;
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            if (doTextEnd)
            {
                OnClickNext();
            }
            else if (!texts[currentTextNum].cantSkipText)
            {
                SkipText();
            }
        }

        nextButtonObj.SetActive(canNextTalk && doTextEnd);
    }

    private void SkipText()
    {
        text.text = "";

        if (texts[currentTextNum].startOtherTextEventOnPlayerSkip && !doTextEnd)
        {
            int pasteTextNum = currentTextNum;
            TextEnd();

            talkManager.currentTextBoxesParent.SpawnTextBox(texts[pasteTextNum].otherTextEventIndex);

            return;
        }

        OnClickNext();
    }

    private void TextEnd()
    {
        eventObjSpawnData.ForEach(objData => objData.eventObject.SetActive(false));

        talkManager.CurrentEvents.Clear();

        if (talkManager.CurrentTalkableObject != null)
        {
            if (!talkManager.CurrentTalkableObject.gameObject.activeSelf)
            {
                talkManager.CurrentTalkableObject.gameObject.SetActive(true);
                talkManager.CurrentTalkableObject.StartFadeIn();
            }
            talkManager.CurrentTalkableObject = null;
        }

        gameManager.player.gameObject.SetActive(true);
        gameManager.cinemachineVirtualCamera.Follow = gameManager.player.transform;

        currentTextNum = 0;

        talkManager.currentTextBoxesParent.DeSpawnTextBox();
    }

    private void OnEnable()
    {
        eventObjSpawnData.ForEach(objData =>
        {
            objData.eventObject.SetActive(true);

            if (!objData.eventObject.GetComponent<TextEventObject>().IsStartAtPlayer)
            {
                objData.eventObject.transform.position = objData.eventObjSpawnPos;
            }

            talkManager.CurrentEvents.Add(objData.eventObject.GetComponent<TextEventObject>());

        });

        doFirstText = true;
    }

    public void SetText() // gameManager의 SetSlowTime이 실행된 상태면 텍스트 설정이 느리게 되는 버그
    {
        if (!doFirstText)
        {
            talkManager.CurrentEvents.ForEach((item) => item.CanDoEvent = true); // hiddenText로 넘어갈 때 두번실행됌
        }

        canNextTalk = texts[currentTextNum].canNextTalk;

        SetSpriteRenderers();

        text.text = "";

        doTextEnd = false;

        float doTextTime = (1 / doTextSpeed) * texts[currentTextNum].contents.Length;
        doTextEndTimer = doTextTime;

        text.DOText(texts[currentTextNum].contents, doTextTime);

        if (texts[currentTextNum].cameraFollowPos != null)
        {
            gameManager.cinemachineVirtualCamera.Follow = texts[currentTextNum].cameraFollowPos;
        }
        else
        {
            gameManager.cinemachineVirtualCamera.Follow = gameManager.player.transform;
        }

        if (SetPlayerRespawnPosition)
        {
            stagemanager.SetPlayerRespawnPosition(gameManager.player.transform.position);
        }
    }
    private void DoTextEndCheck()
    {
        if (doTextEndTimer > 0f)
        {
            doTextEndTimer -= Time.deltaTime;

            if (doTextEndTimer < 0f)
            {
                doTextEnd = true;
            }
        }
    }

    private void SetSpriteRenderers()
    {
        LSpriteRenderer.sprite = texts[currentTextNum].LSprite;
        RSpriteRenderer.sprite = texts[currentTextNum].RSprite;

        // 왼쪽 혹은 오른쪽에 위치하는 sprite가 null일경우 alpha를 0f로 줄여줌
        if (LSpriteRenderer.sprite == null)
        {
            LSpriteRenderer.color = new Vector4(1f, 1f, 1f, 0f);
        }
        else
        {
            LSpriteRenderer.color = new Vector4(1f, 1f, 1f, 1f);
            PlayerSayCheck();
        }

        if (RSpriteRenderer.sprite == null)
        {
            RSpriteRenderer.color = new Vector4(1f, 1f, 1f, 0f);
        }
        else
        {
            RSpriteRenderer.color = new Vector4(1f, 1f, 1f, 1f);
            PlayerSayCheck();
        }
    }

    private void PlayerSayCheck()
    {
        if (texts[currentTextNum].isPlayerSay) // 말하고있는 대상의 alpha는 냅두고 말하고있지 않는 대상의 alpha를 0.5f로 줄여줌.
        {
            text.alignment = TextAnchor.UpperLeft;
            RSpriteRenderer.color = new Vector4(1f, 1f, 1f, 0.5f);
        }
        else
        {
            text.alignment = TextAnchor.UpperRight;
            LSpriteRenderer.color = new Vector4(1f, 1f, 1f, 0.5f);
        }
    }

    public void OnClickNext()
    {
        if (canNextTalk)
        {
            currentTextNum++;

            if (currentTextNum < texts.Count)
            {
                currentTextNum = Mathf.Clamp(currentTextNum, 0, texts.Count - 1);
                canNextTalk = texts[currentTextNum].canNextTalk;

                SetText();
            }
            else
            {
                TextEnd();

                talkManager.CurrentEvents.ForEach((item) =>
                {
                    item.StartFadeOut();
                });


                if (endGameAtEndTalk)
                {
                    gameManager.StageEnd(gameClearAtEndGame);
                }
            }

            if (currentTextNum < texts.Count - 1)
            {
                nextButtonTxt.text = "다음";
            }
            else
            {
                nextButtonTxt.text = "대화 종료";
            }
        }
    }
}
