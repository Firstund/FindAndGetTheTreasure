using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowObjectEventTrigger : MonoBehaviour, IEventTrigger
{
    private GameObject showThis = null;

    void Start()
    {
        showThis = transform.Find("ShowThis").gameObject;

        if(showThis == null)
        {
            Debug.LogError("Can't find the Object Named 'ShowThis' in Childs");
        }
        else
        {
            showThis.SetActive(false);
        }
    }
    public void DoEvent()
    {
        showThis.SetActive(true);
    }

    public void DoEventWhenDoEventFalse()
    {
        showThis.SetActive(false);
    }
}
