using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class Texts : Text_Base
{
    [SerializeField]
    private List<sText> texts = new List<sText>();
    [SerializeField]
    private Text text = null;
    [SerializeField]
    private int currentTextNum = 0;

    void Start()
    {
        SetText();
    }
    public void OnClickNext()
    {
        currentTextNum++;
        currentTextNum = Mathf.Clamp(currentTextNum, 0, texts.Count - 1);

        SetText();
    }
    private void SetText()
    {
        if (texts[currentTextNum].isPlayerSay)
        {
            text.alignment = TextAnchor.UpperLeft;
        }
        else
        {
            text.alignment = TextAnchor.UpperRight;
        }
        text.DOText(texts[currentTextNum].contents, 0.5f);
    }
}
