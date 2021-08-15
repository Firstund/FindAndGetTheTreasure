using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatBarBackground : MonoBehaviour
{
    [Header("Ap, Dp, Hp중 가장 큰 값")]
    [SerializeField]
    private float maxValue = 0f;

    private RectTransform _rectTransform = null;
    public RectTransform rectTransform
    {
        get
        {
            if (_rectTransform == null)
            {
                _rectTransform = GetComponent<RectTransform>();
            }

            return _rectTransform;
        }
    }
    private float firstSizeX = 0f;
    private void Awake()
    {
        firstSizeX = rectTransform.sizeDelta.x;
        SetRects();
    }
        
    public void SetRects()
    {
        rectTransform.sizeDelta = new Vector2(maxValue * firstSizeX, rectTransform.sizeDelta.y);
    }
}
