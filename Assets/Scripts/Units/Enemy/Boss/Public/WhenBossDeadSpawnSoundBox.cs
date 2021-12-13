using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhenBossDeadSpawnSoundBox : MonoBehaviour, IWhenBossDead
{
    private StageManager stageManager = null;

    [SerializeField]
    private GameObject soundBox = null;
    private void Start() 
    {
        stageManager = StageManager.Instance;
    }
    public void DoWhenBossDead()
    {
        stageManager.SpawnSoundBox(soundBox);
    }
}
