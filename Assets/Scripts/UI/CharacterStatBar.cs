using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CharacterStatBar : MonoBehaviour
{
    [SerializeField]
    private string _stat = "";
    public string stat
    {
        get { return _stat; }
    }
    [SerializeField]
    private float doValueTime = 0.5f;
    [SerializeField]
    private RectTransform HPBarBackGroundRect = null;
    private RectTransform rectTransform = null;

    private CharacterStat _characterStat = null;
    public CharacterStat characterStat
    {
        get { return _characterStat; }
    }
    private Slider _hpSlider = null;
    private Slider hpSlider
    {
        get
        {
            if (_hpSlider == null)
            {
                _hpSlider = GetComponent<Slider>();
            }
            return _hpSlider;
        }
    }
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();

        DontDestroyOnLoad(gameObject);
    }

    void FixedUpdate()
    {
        rectTransform.sizeDelta = HPBarBackGroundRect.sizeDelta;
        SetSliderValue();
    }

    private void SetSliderValue()
    {
        switch (stat)
        {
            case "hp":
                if (characterStat != null)
                {
                    hpSlider.DOValue(characterStat.hp, doValueTime);
                }
                break;
        }
    }

    public void GetCharacterStat(CharacterStat a)
    {
        _characterStat = a;

        hpSlider.maxValue = characterStat.hp;
        hpSlider.minValue = 0f;
    }
}
