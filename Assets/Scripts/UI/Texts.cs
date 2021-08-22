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
    private List<sText> texts = new List<sText>();
    [SerializeField]
    private Text text = null;
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
        if(doFirstText)
        {
            SetText();
            doFirstText = false;
        }
    }
    
    public void SetText()
    {
        if (texts[currentTextNum].isPlayerSay)
            {
                text.alignment = TextAnchor.UpperLeft;
            }
            else
            {
                text.alignment = TextAnchor.UpperRight;
            }

            text.text = "";

            text.DOText(texts[currentTextNum].contents, 0.5f);
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
