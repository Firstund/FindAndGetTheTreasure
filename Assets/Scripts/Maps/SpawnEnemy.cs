using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    private GameManager gameManager = null;

    [SerializeField]
    private float enemySpawnRange = 5f;
    
    [SerializeField]
    private Transform enemySpawnPosition = null;
    private bool enemySpawned = false;
    [SerializeField]
    private GameObject spawnThis = null;
    private Vector2 currentPosition = Vector2.zero;

    void Start()
    {
        gameManager = GameManager.Instance;
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

        if(distance <= enemySpawnRange && !enemySpawned)
        {
            enemySpawned = true;
            spawnThis.SetActive(true);
            spawnThis.transform.position = enemySpawnPosition.position;

            spawnThis.transform.SetParent(gameManager.enemys);
            gameObject.SetActive(false);
        }
    }
}
