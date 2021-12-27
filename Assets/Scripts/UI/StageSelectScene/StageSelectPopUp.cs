using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSelectPopUp : PopUpScaleScript
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
    public override void OnDisable()
    {
        base.OnDisable();
    }
}
