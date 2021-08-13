using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterHpBar : MonoBehaviour
{
    private CharacterStat characterStat = null;
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
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        if (characterStat != null)
        {
            hpSlider.value = characterStat.hp;
        }
    }
    public void GetCharacterStat(CharacterStat a)
    {
        characterStat = a;

        hpSlider.maxValue = characterStat.hp;
        hpSlider.minValue = 0f;
    }
}
