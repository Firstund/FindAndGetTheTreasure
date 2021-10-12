using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStatBarParent : MonoBehaviour
{
    [Header("세가지 스탯중 가장 큰 값 혹은 그보다 큰 값 입력")]
    [SerializeField]
    private float maximumStat = 0f;
    [SerializeField]
    private float sizeXPerMaximumStat = 0f;

    private RectTransform rectTransform = null;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();

        SetRects();
    }
     public void SetRects()
    {
        rectTransform.sizeDelta = new Vector2(maximumStat * sizeXPerMaximumStat, rectTransform.sizeDelta.y);    
    }
}
