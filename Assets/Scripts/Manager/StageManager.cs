using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class StageManager : MonoBehaviour
{
    private GameManager gameManager = null;
    private List<GameObject> enemys = new List<GameObject>();

    private float shakeTimer = 0f; // 시네머신을 이용하여 카메라를 흔들 때 사용되는 변수

    [SerializeField]
    private CinemachineVirtualCamera cinemachineVirtualCamera = null;

    void Start()
    {
        gameManager = GameManager.Instance;
    }

    void Update()
    {
        TimerCheck();
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

        if (!enemySpanwed)
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

    public void ShakeCamera(float intensity, float time)
    {
        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity;
        shakeTimer = time;
    }
    private void TimerCheck()
    {
        if (shakeTimer > 0f)
        {
            shakeTimer -= Time.deltaTime;

            if (shakeTimer <= 0f)
            {
                CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

                cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0f;
            }
        }
    }

}
