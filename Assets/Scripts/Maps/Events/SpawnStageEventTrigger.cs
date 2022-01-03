using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnStageEventTrigger : MonoBehaviour, IEventTrigger
{
    private SpawnStageManager spawnStageManager = null;

    [SerializeField]
    private int spawnStageIndex = 0;

    private void Start() 
    {
        spawnStageManager = FindObjectOfType<SpawnStageManager>();
    }

    public void DoEvent()
    {
        spawnStageManager.SpawnNewStage(spawnStageIndex);
    }
    public void DoEventWhenDoEventFalse()
    {
        
    }
}
