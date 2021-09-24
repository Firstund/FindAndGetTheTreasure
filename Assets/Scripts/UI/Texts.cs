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

    [SerializeField]
    private List<SText> texts = new List<SText>();
    [SerializeField]
    private Text text = null;

    [SerializeField]
    private Image LSpriteRenderer = null;
    [SerializeField]
    private Image RSpriteRenderer = null;

    [SerializeField]
    private int currentTextNum = 0;

    [Header("이 값이 true면 대화가 끝났을 때 게임종료처리")]
    [SerializeField]
    private bool endGameAtEndTalk = false;
    [Header("이 값이 true면 게임종료시 게임클리어처리 false면 게임오버처리")]
    [SerializeField]
    private bool gameClearAtEndGame = false;
    private bool doFirstText = true;

    void Start()
    {
        gameManager = GameManager.Instance;
        talkManager = FindObjectOfType<TalkManager>();

        SetText();
    }
    public void Update()
    {
        if (doFirstText)
        {
            SetText();
            doFirstText = false;
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            OnClickNext();
        }
    }

    public void SetText() // gameManager의 SetSlowTime이 실행된 상태면 텍스트 설정이 느리게 되는 버그
    {
        SetSpriteRenderers();

        text.text = "";

        text.DOText(texts[currentTextNum].contents, 0.5f);

        foreach (GameObject obj in texts[currentTextNum].eventObjects)
        {
            obj.SetActive(true);
        }

        if(texts[currentTextNum].cameraFollowPos != null)
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
        if (texts[currentTextNum].canNextTalk)
        {
            foreach (GameObject obj in texts[currentTextNum].eventObjects)
            {
                obj.SetActive(false);
            }

            currentTextNum++;

            if (currentTextNum < texts.Count)
            {
                currentTextNum = Mathf.Clamp(currentTextNum, 0, texts.Count - 1);

                SetText();
            }
            else
            {
                currentTextNum = 0;
                doFirstText = true;
                talkManager.currentTextBoxesParent.DeSpawnTextBox();

                if (endGameAtEndTalk)
                {
                    gameManager.GameEnd(gameClearAtEndGame);
                }
            }
        }
    }
}
