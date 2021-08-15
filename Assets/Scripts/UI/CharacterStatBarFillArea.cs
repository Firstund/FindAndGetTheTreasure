using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStatBarFillArea : MonoBehaviour
{
    [SerializeField]
    private StatBarBackground statBarBackground = null;
    private RectTransform rectTransform = null;
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    private void Update()
    {
        SetRects();
    }

    public void SetRects()
    {
        rectTransform.sizeDelta = new Vector2(statBarBackground.rectTransform.sizeDelta.x, statBarBackground.rectTransform.sizeDelta.y - 10f);
    }
}
