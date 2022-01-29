using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class QuickGameAnswerPopUp : PopUpScaleScript
{
    [SerializeField]
    private bool isApplicationQuick = false;

    public override void Awake()
    {
        base.Awake();
    }
    private void OnEnable() 
    {
        EventManager.StartListening("WhenAnswerQuick", OnShowPopUp);

        EventManager.StartListening("WhenCancelQuick", OnHidePopUp);

    }
    public override void Start()
    {
        base.Start();
    }
    public override void OnDisable()
    {
        base.OnDisable();

        EventManager.StopListening("WhenAnswerQuick", OnShowPopUp);

        EventManager.StopListening("WhenCancelQuick", OnHidePopUp);
    }
    public override void Update()
    {
        base.Update();
    }
    private void SetActiveFalse()
    {

    }
    public void Quick()
    {
        if (isApplicationQuick)
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
        else
        {
            GetOutStage();
        }
    }
    private void GetOutStage()
    {
        EventManager.TriggerEvent_Bool("StageEnd", false);
        gameManager.StopTime(false);
        EventManager.TriggerEvent("WhenGoToStageSelectMenu");
        SceneManager.LoadScene("StageSelectScene");
    }
}
