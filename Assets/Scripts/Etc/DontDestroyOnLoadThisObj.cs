using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoadThisObj : MonoBehaviour
{
    private DontDestroyOnLoadManager dontDestroyOnLoadManager = null;
    
    void Start()
    {
        dontDestroyOnLoadManager = DontDestroyOnLoadManager.Instance;

        dontDestroyOnLoadManager.DoNotDestroyOnLoad(gameObject);
    }
}
