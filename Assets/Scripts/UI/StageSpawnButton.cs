using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSpawnButton : MonoBehaviour
{
    private GameManager gameManager = null;
    void Start()
    {
        gameManager = GameManager.Instance;
    }
    public void OnClick(int stage)
    {
        if(gameManager == null)
        {
            gameManager = GameManager.Instance;
        }

        gameManager.SpawnStage(stage);
    }
}
