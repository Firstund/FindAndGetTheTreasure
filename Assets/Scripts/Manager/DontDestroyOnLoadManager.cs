using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoadManager : MonoBehaviour
{
    private static DontDestroyOnLoadManager instance;
    public static DontDestroyOnLoadManager Instance
    {
        get{
            if(instance == null)
            {
                instance = FindObjectOfType<DontDestroyOnLoadManager>();

                if(instance == null)
                {
                    GameObject temp = new GameObject("DontDestroyOnLoadManager");
                    instance = temp.AddComponent<DontDestroyOnLoadManager>();
                }
            }

            return instance;
        }
    }

    [SerializeField]
    private List<GameObject> dontDestroyOnLoadObjectList = new List<GameObject>();
    private void Start()
    {
        DoNotDestroyOnLoad(gameObject);
    }
    public void DoNotDestroyOnLoad(GameObject donDestroyOnLoadObject)
    {
        foreach(var item in dontDestroyOnLoadObjectList)
        {
            if(item.name == donDestroyOnLoadObject.name)
            {
                Destroy(donDestroyOnLoadObject);
                return;
            }
        }

        dontDestroyOnLoadObjectList.Add(donDestroyOnLoadObject);
        DontDestroyOnLoad(donDestroyOnLoadObject);
    }
}
