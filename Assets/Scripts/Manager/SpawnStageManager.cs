using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnStageManager : MonoBehaviour
{
    private GameManager gameManager = null;
    private GameObject currentStage = null;
    void Start()
    {
        gameManager = GameManager.Instance;

        currentStage = Instantiate(gameManager.stages[gameManager.currentStageNum]);
    }

    public void SpawnNewStage(int stageNum) // 스테이지 안에서 다른 스테이지를 부를 때 사용
    {
        Destroy(currentStage);

        currentStage = Instantiate(gameManager.stages[stageNum]);
        gameManager.SpawnStage(stageNum);
    }
}
