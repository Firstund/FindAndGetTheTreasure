using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEffect : MonoBehaviour
{
    private StageManager stageManager = null;
    [SerializeField]
    private Transform effectSpawnPosition = null;
    [SerializeField]
    private List<GameObject> effects = new List<GameObject>();

    private void Start()
    {
        stageManager = FindObjectOfType<StageManager>();
    }
    public void ShowEffect(GameObject spawnIt)
    {
        Transform spawnTrm = effectSpawnPosition;
        bool findEffect = false;

        if (spawnIt.GetComponent<EffectScript>().IsSpawnOnWorld)
        {
            spawnTrm = stageManager.transform;
        }

        foreach (var item in effects)
        {
            if (item.name == spawnIt.name + "(Clone)" && !item.activeSelf)
            {
                findEffect = true;
                item.transform.position = effectSpawnPosition.position;
                item.SetActive(true);

                break;
            }
        }

        if (!findEffect)
        {
            GameObject obj = Instantiate(spawnIt, spawnTrm);
            effects.Add(obj);

            obj.transform.position = effectSpawnPosition.position;
        }
    }
}
