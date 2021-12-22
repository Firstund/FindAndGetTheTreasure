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
        menuManager.OnShowMenu += () => 
        {
            OnShowPopUp();
        };

        menuManager.OnHideMenu += () =>
        {
            OnHidePopUp();
        };
    }
    public override void Start()
    {
        base.Start();

        gameObject.SetActive(false);
    }
    public override void OnDisable()
    {
        base.OnDisable();  

        menuManager.OnShowMenu -= () =>
        {
            OnShowPopUp();
        };

        menuManager.OnHideMenu -= () =>
        {
            OnHidePopUp(); 
        };
    }
    public override void Update()
    {
        base.Update();
    }
}
