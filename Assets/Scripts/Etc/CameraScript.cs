using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    private DontDestroyOnLoadManager dontDestroyOnLoadManager = null;
    void Start()
    {
        dontDestroyOnLoadManager = DontDestroyOnLoadManager.Instance;
        
        dontDestroyOnLoadManager.DoNotDestroyOnLoad(gameObject);
    }
}
