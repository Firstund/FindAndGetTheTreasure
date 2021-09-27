using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Cinemachine;

public class StageManager : MonoBehaviour
{
    private GameManager gameManager = null;
    private List<GameObject> enemys = new List<GameObject>();
    public List<GameObject> Enemys
    {
        get { return enemys; }
    }

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
    [SerializeField]
    private Transform playerRespanwTrm = null;
    private Transform PlayerRespawnTrm
    {
        get
        {
            if (playerRespanwTrm == null)
            {
                playerRespanwTrm = GameObject.Find("PlayerRespawnTrm").transform;
            }

            return playerRespanwTrm;
        }
    }

    [SerializeField]
    private float respawnDelay = 2f;
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

        // if(playerRespanwTrm == null)
        // {
        //     playerRespanwTrm = GameObject.Find("PlayerRespawnTrm").transform;
        // }
    }

    void Update()
    {
        shakeTimer = TimerCheck(shakeTimer);
        // if (Input.GetKeyDown(KeyCode.A))
        // {
        //     SpawnSoundBox(TestSoundBox);
        // }
    }
    public void ShootProjectile(GameObject shootIt, EnemyStat enemyStat, bool flipX, Vector2 spawnPosition, float shootRange)
    {
        GameObject shootObject = projectiles.Find(x => (x.name == shootIt.name + "(Clone)") && !x.activeSelf);

        Vector2 shootDir = flipX ? Vector2.left : Vector2.right;

        if (shootObject == null)
        {
            GameObject a = Instantiate(shootIt, projectilesTrm);
            a.GetComponent<EnemyProjectile>().SpawnSet(shootRange, enemyStat.ap, shootDir);
            a.transform.position = spawnPosition;
        }
        else
        {
            shootObject.SetActive(true);
            shootObject.GetComponent<EnemyProjectile>().SpawnSet(shootRange, enemyStat.ap, shootDir);
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
        GameObject spawnEnemy = enemys.Find(x => (x.name == spawnObject.name + "(Clone)") && (!x.activeSelf));

        if (spawnEnemy == null)
        {
            GameObject a = Instantiate(spawnObject, _enemys);
            a.transform.position = spawnPosition;
            enemys.Add(a);
        }
        else
        {
            spawnEnemy.transform.position = spawnPosition;
            spawnEnemy.SetActive(true);
        }
    }
    public void DespawnEnemy(GameObject deSpawnObject)
    {
        deSpawnObject.SetActive(false);
    }
    public GameObject SpawnSoundBox(GameObject spawnIt)
    {
        GameObject spawnSoundBox = soundBoxes.Find(x => x.name == spawnIt.name + "(Clone)");

        if (spawnSoundBox == null)
        {
            GameObject a = Instantiate(spawnIt, _soundBoxes);

            return a;
        }
        else
        {
            spawnSoundBox.SetActive(true);
            soundBoxes.Remove(spawnSoundBox);

            return spawnSoundBox;
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
    public void SetPlayerRespawnPosition(Vector2 respawnPos)
    {
        PlayerRespawnTrm.position = respawnPos;
    }
    public void Respawn()
    {
        Invoke("PlayerRespawn", respawnDelay);
    }
    public void PlayerRespawn()
    {
        gameManager.player.gameObject.SetActive(true);
        gameManager.player.transform.position = PlayerRespawnTrm.position;
    }
}
