using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhenBossDeadSpawnObj : MonoBehaviour, IWhenBossDead
{
    private GameManager gameManager = null;
    private StageManager stageManager = null;

    [SerializeField]
    private GameObject spawnIt = null;
    [SerializeField]
    private Transform spawnPos = null;

    private void Start()
    {
        gameManager = GameManager.Instance;
        stageManager = StageManager.Instance;
    }
    public void DoWhenBossDead()
    {
        if (!gameManager.IsGameEnd)
        {
            stageManager.SpawnObject(spawnIt, spawnPos.position);
        }
    }
}
