using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSystemScript : MonoBehaviour
{
    DontDestroyOnLoadManager dontDestroyOnLoadManager = null;
    void Start()
    {
        dontDestroyOnLoadManager = DontDestroyOnLoadManager.Instance;
        
        dontDestroyOnLoadManager.DoNotDestroyOnLoad(gameObject);
    }
}
