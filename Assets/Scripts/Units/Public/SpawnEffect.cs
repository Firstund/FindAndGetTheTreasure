using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEffect : MonoBehaviour
{
    private SpriteRenderer spriteRenderer = null;
    private StageManager stageManager = null;
    [SerializeField]
    private Transform effectSpawnPosition = null;
    [SerializeField]
    private List<GameObject> effects = new List<GameObject>();

    private void Start()
    {
       stageManager = StageManager.Instance;

        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public void ShowEffect(GameObject spawnIt, Vector2 spawnPosition)
    {
        Transform spawnTrm = effectSpawnPosition;
        SpriteRenderer renderer = null;

        bool findEffect = false;

        if (spawnIt.GetComponent<EffectScript>().IsSpawnOnWorld)
        {
            spawnTrm = stageManager.transform;
        }

        if (spawnPosition != Vector2.zero)
        {
            spawnTrm.position = spawnPosition;
        }

        foreach (var item in effects)
        {
            if (item.name == spawnIt.name + "(Clone)" && !item.activeSelf)
            {
                findEffect = true;

                item.transform.position = spawnTrm.position;

                renderer = item.GetComponent<SpriteRenderer>();

                if (renderer != null)
                {
                    renderer.flipX = spriteRenderer.flipX;
                }

                item.SetActive(true);

                break;
            }
        }

        if (!findEffect)
        {
            GameObject obj = Instantiate(spawnIt, spawnTrm);
            effects.Add(obj);

            obj.transform.position = spawnTrm.position;

            renderer = obj.GetComponent<SpriteRenderer>();

            if (renderer != null)
            {
                renderer.flipX = spriteRenderer.flipX;
            }
        }
    }
}
