using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectScript : MonoBehaviour
{
    [SerializeField]
    private float destroyTimer = 5f;
    private float timer = 0f;

    [SerializeField]
    private bool isSpawnOnWorld = false;
    public bool IsSpawnOnWorld
    {
        get { return isSpawnOnWorld; }
    }
    void Start()
    {
        timer = destroyTimer;

    }
    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            Destroye();
        }
    }
    private void Destroye()
    {
        timer = destroyTimer;
        gameObject.SetActive(false);
    }
}
