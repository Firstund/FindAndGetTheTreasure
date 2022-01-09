using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScriptBase : PopUpScaleScript
{
    public override void Awake()
    {
        base.Awake();
    }
    private void OnEnable()
    {
        EventManager.StartListening("OnShowMenu", OnShowPopUp);
        EventManager.StartListening("OnHideMenu", OnHidePopUp);
    }
    public override void Start()
    {
        base.Start();

        gameObject.SetActive(false);
    }
    public override void OnDisable()
    {
        base.OnDisable();

        EventManager.StopListening("OnShowMenu", OnShowPopUp);
        EventManager.StopListening("OnHideMenu", OnHidePopUp);
    }
    public override void Update()
    {
        base.Update();
    }
}
