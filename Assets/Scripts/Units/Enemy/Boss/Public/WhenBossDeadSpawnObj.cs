using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhenBossDeadSpawnObj : MonoBehaviour, IWhenBossDead
{
    private GameManager gameManager = null;

    [SerializeField]
    private GameObject spawnIt = null;

    private void Start()
    {
        gameManager = GameManager.Instance;
    }
    public void DoWhenBossDead()
    {
        if (!gameManager.IsGameEnd)
        {
            StageManager.Instance.SpawnObject(spawnIt, transform.position); // 보스가 한번 죽고나서 스테이지 나갔다가 다시 들어오면 transform.position이 null값이 되는 이상한 에러
        }
    }
}
