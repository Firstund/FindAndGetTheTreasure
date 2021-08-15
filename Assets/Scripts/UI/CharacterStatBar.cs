using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CharacterStatBar : MonoBehaviour
{
    private GameManager gameManager = null;

    [SerializeField]
    private string _stat = "";
    public string stat
    {
        get { return _stat; }
    }
    [SerializeField]
    private float doValueTime = 0.5f;

    [Header("Ap, Dp, Hp중 가장 큰 값")]
    [SerializeField]
    private float maxValue = 0f;
    [SerializeField]
    private RectTransform StatBarBackgroundRect = null;
    private RectTransform rectTransform = null;

    private CharacterStat _characterStat = null;
    public CharacterStat characterStat
    {
        get
        {
            if (_characterStat == null)
            {
                _characterStat = gameManager.characterStat;
            }

            return _characterStat;
        }
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
        gameManager = GameManager.Instance;

        rectTransform = GetComponent<RectTransform>();

        GetCharacterStat();
    }

    void FixedUpdate()
    {
        rectTransform.sizeDelta = new Vector2(StatBarBackgroundRect.sizeDelta.x, rectTransform.sizeDelta.y);
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

            case "ap":
                if (characterStat != null)
                {
                    hpSlider.DOValue(characterStat.ap, doValueTime);
                }
                break;
            case "dp":
                if (characterStat != null)
                {
                    hpSlider.DOValue(characterStat.dp, doValueTime);
                }
                break;
        }
    }

    public void GetCharacterStat()
    {
        hpSlider.maxValue = maxValue;
        hpSlider.minValue = 0f;
    }
}
