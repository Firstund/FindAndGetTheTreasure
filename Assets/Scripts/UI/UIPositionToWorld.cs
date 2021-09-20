using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPositionToWorld : MonoBehaviour
{
    private GameManager gaemManager = null;
    private RectTransform rectTransform = null;
    [SerializeField]
    private Transform follow = null;
    void Start()
    {
        gaemManager = GameManager.Instance;

        rectTransform = GetComponent<RectTransform>();
    }
    void Update()
    {
        rectTransform.position = Camera.main.WorldToScreenPoint(follow.position);
    }
}
