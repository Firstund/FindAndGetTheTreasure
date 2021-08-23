using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using DG.Tweening;


public class Texts : Text_Base
{
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
    private bool doFirstText = true;

    void Start()
    {
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

        if(Input.GetButtonUp("Jump"))
        {
            OnClickNext();
        }
    }

    public void SetText()
    {
        SetSpriteRenderers();

        if (texts[currentTextNum].isPlayerSay)
        {
            text.alignment = TextAnchor.UpperLeft;
            RSpriteRenderer.color = new Vector4(1f, 1f, 1f, 0.5f);
        }
        else
        {
            text.alignment = TextAnchor.UpperRight;
            LSpriteRenderer.color = new Vector4(1f, 1f, 1f, 0.5f);
        }

        text.text = "";

        text.DOText(texts[currentTextNum].contents, 0.5f);
    }

    private void SetSpriteRenderers()
    {
        LSpriteRenderer.sprite = texts[currentTextNum].LSprite;
        RSpriteRenderer.sprite = texts[currentTextNum].RSprite;

        if (LSpriteRenderer.sprite == null)
        {
            LSpriteRenderer.color = new Vector4(1f, 1f, 1f, 0f);
        }
        else
        {
            LSpriteRenderer.color = new Vector4(1f, 1f, 1f, 1f);
        }

        if (RSpriteRenderer.sprite == null)
        {
            RSpriteRenderer.color = new Vector4(1f, 1f, 1f, 0f);
        }
        else
        {
            RSpriteRenderer.color = new Vector4(1f, 1f, 1f, 1f);
        }
    }

    public void OnClickNext()
    {
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
        }
    }


}
