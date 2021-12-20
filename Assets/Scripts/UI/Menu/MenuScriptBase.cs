using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScriptBase : PopUpScaleScript
{

    public override void Awake() 
    {
        base.Awake();
    }
    public override void Start()
    {
        base.Start();

        gameObject.SetActive(false);
    }

    public override void Update()
    {
        base.Update();
    }
}
