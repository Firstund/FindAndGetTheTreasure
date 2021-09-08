using System.Collections;
using System.Collections.Generic;
using System;
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
    [SerializeField]
    private List<GameObject> projectiles = new List<GameObject>();
    [SerializeField]
    private Transform projectilesTrm = null;
    public Transform ProjectilesTrm
    {
        get { return projectilesTrm; }
    }

    private float shakeTimer = 0f; // 시네머신을 이용하여 카메라를 흔들 때 사용되는 변수

    private float originOrthographicSize = 0f;

    [SerializeField]
    private CinemachineVirtualCamera cinemachineVirtualCamera = null;
    [SerializeField]
    private GameObject TestSoundBox = null;

    private Func<float, float> TimerCheck;

    private void Awake()
    {
        TimerCheck = t =>
        {
            if (t > 0f)
            {
                t -= Time.deltaTime;

                if (t <= 0f)
                {
                    CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

                    cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0f;
                }

            }

            return t;

        };

    }

    void Start()
    {
        gameManager = GameManager.Instance;

        cinemachineVirtualCamera = gameManager.cinemachineVirtualCamera;
        cinemachineVirtualCamera.m_Follow = gameManager.player.transform;

        originOrthographicSize = cinemachineVirtualCamera.m_Lens.OrthographicSize;

    }

    void Update()
    {
        shakeTimer = TimerCheck(shakeTimer);
        if (Input.GetKeyDown(KeyCode.A))
        {
            SpawnSoundBox(TestSoundBox);
        }
    }
    public void ShootProjectile(GameObject shootIt, EnemyStat enemyStat, bool flipX, Vector2 spawnPosition, float shootRange)
    {
        GameObject shootObject = projectiles.Find(x => (x.name == shootIt.name + "(Clone)") && !x.activeSelf);

        Vector2 shootDir = flipX ? Vector2.left : Vector2.right;

        if (shootObject == null)
        {
            GameObject a = Instantiate(shootIt, projectilesTrm);
            a.GetComponent<EnemyProjectile>().SpawnSet(flipX, shootRange, enemyStat.ap, shootDir);
            a.transform.position = spawnPosition;
        }
        else
        {
            shootObject.SetActive(true);
            shootObject.GetComponent<EnemyProjectile>().SpawnSet(flipX, shootRange, enemyStat.ap, shootDir);
            shootObject.transform.position = spawnPosition;
            projectiles.Remove(shootObject);
        }
    }
    public void DespawnProjectile(GameObject deSpawnProjectile)
    {
        projectiles.Add(deSpawnProjectile);
        deSpawnProjectile.SetActive(false);
    }

    public void SpawnEnemy(GameObject spawnObject, Vector2 spawnPosition)
    {
        GameObject spawnEnemy = enemys.Find(x => x.name == spawnObject.name + "(Clone)");

        if (spawnEnemy == null)
        {
            GameObject a = Instantiate(spawnObject, _enemys);
            a.transform.position = spawnPosition;
        }
        else
        {
            spawnEnemy.SetActive(true);
            spawnEnemy.GetComponent<EnemyMove>().SpawnSet();
            spawnEnemy.transform.position = spawnPosition;
            enemys.Remove(spawnEnemy);
        }
    }
    public void DespawnEnemy(GameObject deSpawnObject)
    {
        enemys.Add(deSpawnObject);
        deSpawnObject.SetActive(false);
    }
    public void SpawnSoundBox(GameObject spawnIt)
    {
        GameObject spawnSoundBox = soundBoxes.Find(x => x.name == spawnIt.name + "(Clone)");

        if (spawnSoundBox == null)
        {
            GameObject a = Instantiate(spawnIt, _soundBoxes);
        }
        else
        {
            spawnSoundBox.SetActive(true);
            soundBoxes.Remove(spawnSoundBox);
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
    public IEnumerator SetCameraSize(float orthographicSize, float time)
    {
        cinemachineVirtualCamera.m_Lens.OrthographicSize = orthographicSize;

        yield return new WaitForSeconds(time);

        cinemachineVirtualCamera.m_Lens.OrthographicSize = originOrthographicSize;
    }

}
