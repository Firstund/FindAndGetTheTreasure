using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatBarBackground : MonoBehaviour
{
    [SerializeField]
    private string statName = "";
    [SerializeField]
    private float stat = 0f;

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
        switch (statName)
        {
            case "hp":
                rectTransform.sizeDelta = new Vector2(stat * firstSizeX, rectTransform.sizeDelta.y);
                break;
        }
    }

    void Update()
    {

    }
}
