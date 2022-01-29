using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowQuickGameAnswerPopUp : MonoBehaviour
{
    [SerializeField]
    private GameObject AnswerQuickPopUp = null;

    private void Awake()
    {
        EventManager.StartListening("WhenAnswerQuick", () =>
        {

        });
        EventManager.StartListening("WhenCancelQuick", () =>
        {

        });
    }

    public void AnswerQuick()
    {
        AnswerQuickPopUp.SetActive(true);

        EventManager.TriggerEvent("WhenAnswerQuick");
    }
    public void CancelQuick()
    {
        EventManager.TriggerEvent("WhenCancelQuick");
    }
}
