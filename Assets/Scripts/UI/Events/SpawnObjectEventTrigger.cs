using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObjectEventTrigger : MonoBehaviour, IEventTrigger
{
    private GameManager gameManager = null;
    [SerializeField]
    private GameObject spawnObject = null;

    [SerializeField]
    private Transform spawnPosition = null;
    private void Awake() 
    {
        gameManager = GameManager.Instance;
    }
    public void DoEvent()
    {
        if(!gameManager.IsGameEnd)
        {
            GameObject o = Instantiate(spawnObject, gameManager.SpawnObjByTriggerTrm);

            o.transform.position = spawnPosition.position;
        }
    }
}
