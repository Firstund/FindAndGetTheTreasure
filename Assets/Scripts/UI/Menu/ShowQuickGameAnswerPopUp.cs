using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowQuickGameAnswerPopUp : MonoBehaviour
{
    [SerializeField]
    private GameObject AnswerQuickPopUp = null;

    public event Action WhenAnswerQuick;
    public event Action WhenCancelQuick;

    private void Awake()
    {
        WhenAnswerQuick = () =>
        {

        };
        WhenCancelQuick = () =>
        {

        };
    }

    public void AnswerQuick()
    {
        AnswerQuickPopUp.SetActive(true);

        WhenAnswerQuick?.Invoke();
    }
    public void CancelQuick()
    {
        WhenCancelQuick?.Invoke();
    }
}
