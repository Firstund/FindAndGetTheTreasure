using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnStageManager : MonoBehaviour
{
    private GameManager gameManager = null;
    void Start()
    {
        gameManager = GameManager.Instance;

        Instantiate(gameManager.stages[gameManager.currentStage - 1]);
    }
}
