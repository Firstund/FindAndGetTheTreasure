using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowPopUpEventTrigger : MonoBehaviour, IEventTrigger
{
    [SerializeField]
    private PopUpScaleScript popUp = null;
    private bool popUpShowed = false;

    public void DoEvent()
    {
        if (popUpShowed)
        {
            popUp.OnHidePopUp();
        }
        else
        {
            popUp.gameObject.SetActive(true);
            
            popUp.OnShowPopUp();
        }

        popUpShowed = !popUpShowed;
    }
    public void DoEventWhenDoEventFalse()
    {
        
    }
}
