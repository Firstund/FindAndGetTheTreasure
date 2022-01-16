using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class QuickGameAnswerPopUp : PopUpScaleScript
{
    [SerializeField]
    private ShowQuickGameAnswerPopUp showQuickGameAnswerPopUp = null;

    [SerializeField]
    private bool isApplicationQuick = false;

    public override void Awake()
    {
        base.Awake();
    }
    private void OnEnable() 
    {
        showQuickGameAnswerPopUp.WhenAnswerQuick += OnShowPopUp;

        showQuickGameAnswerPopUp.WhenCancelQuick += OnHidePopUp;
    }
    public override void Start()
    {
        base.Start();
    }
    public override void OnDisable()
    {
        base.OnDisable();

        showQuickGameAnswerPopUp.WhenAnswerQuick -= OnShowPopUp;

        showQuickGameAnswerPopUp.WhenCancelQuick -= OnHidePopUp;
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
        gameManager.StageEnd(false);
        gameManager.StopTime(false);
        gameManager.WhenGoToStageSelectMenu();
        SceneManager.LoadScene("StageSelectScene");
    }
}
