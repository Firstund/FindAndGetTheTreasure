using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class StageManager : MonoBehaviour
{
    private GameManager gameManager = null;
    private List<GameObject> enemys = new List<GameObject>();
    [SerializeField]
    private Transform _enemys = null;

    private List<GameObject> soundBoxes = new List<GameObject>();
    [SerializeField]
    private Transform _soundBoxes = null;

    private List<GameObject> projectiles = new List<GameObject>();
    [SerializeField]
    private Transform _projectiles = null;

    private float shakeTimer = 0f; // 시네머신을 이용하여 카메라를 흔들 때 사용되는 변수

    [SerializeField]
    private CinemachineVirtualCamera cinemachineVirtualCamera = null;
    [SerializeField]
    private GameObject TestSoundBox = null;

    void Start()
    {
        gameManager = GameManager.Instance;
        
    }

    void Update()
    {
        TimerCheck();
        if(Input.GetKeyDown(KeyCode.A))
        {
            SpawnSoundBox(TestSoundBox);
            Debug.Log("Aaa");
        }
    }
    public void ShootProjectile(GameObject shootIt, EnemyStat enemyStat, bool flipX, Vector2 spawnPosition, float shootRange)
    {
        bool shoot = false;

        if(projectiles.Count > 0f)
        {
            foreach(var item in projectiles)
            {
                if(item.name == shootIt.name + "(Clone)")
                {
                    item.SetActive(true);
                    item.GetComponent<EnemyProjectile>().SpawnSet(flipX, shootRange, enemyStat.ap);
                    item.transform.position = spawnPosition;
                    projectiles.Remove(item);
                    shoot = true;
                    break;
                }
            }
        }

        if(!shoot)
        {
            GameObject a = Instantiate(shootIt, _projectiles);
            a.GetComponent<EnemyProjectile>().SpawnSet(flipX , shootRange, enemyStat.ap);
            a.transform.position = spawnPosition;
        }
    }
    public void DespawnProjectile(GameObject deSpawnProjectile)
    {
        projectiles.Add(deSpawnProjectile);
        deSpawnProjectile.SetActive(false);
    }

    public void SpawnEnemy(GameObject spawnObject, Vector2 spawnPosition)
    {
        bool enemySpanwed = false;

        if (enemys.Count > 0)
        {
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

        if (!enemySpanwed)
        {
            GameObject a = Instantiate(spawnObject, _enemys);
            a.transform.position = spawnPosition;
        }
    }
    public void DespawnEnemy(GameObject deSpawnObject)
    {
        enemys.Add(deSpawnObject);
        deSpawnObject.SetActive(false);
    }
    public void SpawnSoundBox(GameObject spawnIt)
    {
        bool soundBoxSpawned = false;

        if (soundBoxes.Count > 0)
        {
            foreach (var item in soundBoxes)
            {
                if (item.name == spawnIt.name + "(Clone)")
                {
                    item.SetActive(true);
                    soundBoxes.Remove(item);
                    soundBoxSpawned = true;
                    Debug.Log("bbb");
                    break;
                }
            }
        }

        if (!soundBoxSpawned)
        {
            GameObject a = Instantiate(spawnIt, _soundBoxes);
        }
    }
    public void DesapwnSoundBox(GameObject despawnIt)
    {
        soundBoxes.Add(despawnIt);
        despawnIt.SetActive(false);
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
    public Vector2 PositionCantCrossWall(Vector2 originPosition, Vector2 endPosition, bool flipX, LayerMask whatIsGround)
    {
        bool goUp = endPosition.y > originPosition.y;
        bool a = false;
        bool b = false;
        bool c = false;

        do
        {
            a = Physics2D.OverlapCircle(originPosition, 0.1f, whatIsGround);
            if (!a)
            {
                if (!b)
                {
                    if (flipX)
                    {
                        if (originPosition.x > endPosition.x)
                        {
                            originPosition.x -= 0.1f;
                        }
                    }
                    else
                    {
                        if (originPosition.x < endPosition.x)
                        {
                            originPosition.x += 0.1f;
                        }
                    }
                }

                if (!c)
                {
                    if (goUp)
                    {
                        if (originPosition.y < endPosition.y)
                        {
                            originPosition.y += 0.1f;
                        }
                    }
                    else
                    {
                        if (originPosition.y > endPosition.y)
                        {
                            originPosition.y -= 0.1f;
                        }
                    }
                }
            }


            if (flipX)
            {
                if (originPosition.x <= endPosition.x)
                {
                    b = true;
                }
            }
            else
            {
                if (originPosition.x >= endPosition.x)
                {
                    b = true;
                }
            }

            if (goUp)
            {
                if (originPosition.y >= endPosition.y)
                {
                    c = true;
                }
            }
            else
            {
                if (originPosition.y <= endPosition.y)
                {
                    c = true;
                }
            }


        } while (!a && (!b || !c));


        return originPosition;
    }

}
