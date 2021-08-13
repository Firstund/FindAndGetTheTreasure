using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatBarBackground : MonoBehaviour
{    
    private CharacterStatBar characterStatBar = null;
    private CharacterStat characterStat = null;
    private RectTransform _rectTransform = null;
    public RectTransform rectTransform
    {
        get { return _rectTransform; }
    }
    void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        characterStatBar = transform.parent.GetComponent<CharacterStatBar>();
    }

    public void SetRects()
    {
        characterStat = characterStatBar.characterStat;

        switch (characterStatBar.stat)
        {
            case "hp":
                rectTransform.sizeDelta = new Vector2(characterStat.hp * 32, rectTransform.sizeDelta.y);
                rectTransform.anchoredPosition = new Vector2((-65) + characterStat.hp * 14.5f, 0);
                break;
        }


    }

    void Update()
    {

    }
}
