using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    private GameManager gameManager = null;
    private StageManager stageManager = null;

    [SerializeField]
    private float enemySpawnRange = 5f;
    private bool enemySpawned = false;

    [SerializeField]
    private GameObject spawnThis = null;
    private Vector2 currentPosition = Vector2.zero;

    void Start()
    {
        gameManager = GameManager.Instance;
        stageManager = StageManager.Instance;
    }
    void FixedUpdate()
    {
        currentPosition = transform.position;

        Spawn();

        transform.position = currentPosition;
    }
    private void Spawn()
    {
        float distance = Vector2.Distance(gameManager.player.transform.position, currentPosition);

        if (distance <= enemySpawnRange && !enemySpawned)
        {
            stageManager.SpawnEnemy(spawnThis, currentPosition);
            enemySpawned = true;
            gameObject.SetActive(false);
        }
    }
}
