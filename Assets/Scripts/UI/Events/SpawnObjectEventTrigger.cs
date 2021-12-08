using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObjectEventTrigger : MonoBehaviour, IEventTrigger
{
    private GameManager gameManager = null;
    private StageManager stageManager = null;
    [SerializeField]
    private GameObject spawnObject = null;

    [SerializeField]
    private Transform spawnPosition = null;
    private void Awake() 
    {
        gameManager = GameManager.Instance;
        stageManager = StageManager.Instance;
    }
    public void DoEvent()
    {
        if(!gameManager.IsGameEnd)
        {
            stageManager.SpawnObject(spawnObject, spawnPosition.position);
        }
    }
}
