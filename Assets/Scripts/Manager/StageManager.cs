using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    private GameManager gameManager = null;
    private List<GameObject> enemys = new List<GameObject>();
    void Start()
    {
        gameManager = GameManager.Instance;
    }

    public void SpawnEnemy(GameObject spawnObject, Vector2 spawnPosition)
    {
        bool enemySpanwed = false;

        if (enemys.Count > 0)
        {
        Debug.Log(enemys[0].name);

            foreach (var item in enemys)
            {
                if (item.name == spawnObject.name + "(Clone)")
                {
                    item.SetActive(true);
                    item.GetComponent<EnemyMove>().SpawnSet();
                    item.transform.position = spawnPosition;
                    enemys.Remove(item);
                    enemySpanwed = true;
                    break;
                }
            }
        }

        Debug.Log(spawnObject.name);

        if(!enemySpanwed)
        {
            GameObject a = Instantiate(spawnObject, gameManager.enemys);
            a.transform.position = spawnPosition;
        }
    }
    public void DespawnEnemy(GameObject deSpawnObject)
    {
        enemys.Add(deSpawnObject);
        deSpawnObject.SetActive(false);
    }
}
