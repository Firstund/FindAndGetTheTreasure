using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using DG.Tweening;


public class Texts : Text_Base
{
    private GameManager gameManager = null;
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

    [Header("이 값이 true면 대화가 끝났을 때 게임종료처리")]
    [SerializeField]
    private bool endGameAtEndTalk = false;
    [Header("이 값이 true면 게임종료시 게임클리어처리 false면 게임오버처리")]
    [SerializeField]
    private bool gameClearAtEndGame = false;
    private bool doFirstText = true;
    public bool canNextTalk = true;

    void Awake()
    {
        gameManager = GameManager.Instance;
        talkManager = TalkManager.Instance;

        nextButtonTxt = nextButtonObj.GetComponentInChildren<Text>();
    }
    public void Update()
    {
        if (doFirstText)
        {
            SetText();

            if (eventObjSpawnData[0].eventObject != null)
            {
                eventObjSpawnData[0].eventObject.transform.position = new Vector2(eventObjSpawnData[0].eventObject.transform.position.x, gameManager.player.currentPosition.y);
            }
            gameManager.player.gameObject.SetActive(false);

            doFirstText = false;
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            OnClickNext();
        }

        nextButtonObj.SetActive(canNextTalk);
    }
    private void OnEnable()
    {
        foreach (SEventObjSpawnData objData in eventObjSpawnData)
        {
            objData.eventObject.SetActive(true);
            objData.eventObject.transform.position = objData.eventObjSpawnPos;

            talkManager.CurrentEvents.Enqueue(objData.eventObject.GetComponent<TextEventObject>());
        }
    }

    public void SetText() // gameManager의 SetSlowTime이 실행된 상태면 텍스트 설정이 느리게 되는 버그
    {
        TextEventObject[] currentEvents = talkManager.CurrentEvents.ToArray();

        for (int i = 0; i < currentEvents.Length; i++)
        {
            currentEvents[i].CanDoEvent = true;
        }

        canNextTalk = texts[currentTextNum].canNextTalk;

        SetSpriteRenderers();

        text.text = "";

        text.DOText(texts[currentTextNum].contents, 0.5f);

        if (texts[currentTextNum].cameraFollowPos != null)
        {
            gameManager.cinemachineVirtualCamera.Follow = texts[currentTextNum].cameraFollowPos;
        }
        else
        {
            gameManager.cinemachineVirtualCamera.Follow = gameManager.player.transform;
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
                talkManager.CurrentTalkableObject.gameObject.SetActive(true);
                talkManager.CurrentTalkableObject.StartFadeIn();
                talkManager.CurrentTalkableObject = null;

                gameManager.player.gameObject.SetActive(true);
                gameManager.cinemachineVirtualCamera.Follow = gameManager.player.transform;
                gameManager.player.transform.position = eventObjSpawnData[0].eventObject.transform.position;

                currentTextNum = 0;
                doFirstText = true;

                while (talkManager.CurrentEvents.Count > 0)
                {
                    talkManager.CurrentEvents.Dequeue().StartFadeOut();
                }

                talkManager.currentTextBoxesParent.DeSpawnTextBox();

                if (endGameAtEndTalk)
                {
                    gameManager.GameEnd(gameClearAtEndGame);
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
