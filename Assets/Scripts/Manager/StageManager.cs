using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Cinemachine;

public class StageManager : MonoBehaviour
{
    private static StageManager instance = null;
    public static StageManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<StageManager>();

                if (instance == null)
                {
                    GameObject temp = new GameObject("StageManager");
                    instance = temp.AddComponent<StageManager>();
                }
            }

            return instance;
        }
    }
    private GameManager gameManager = null;
    private SoundManager soundManager = null;

    private Camera mainCamera = null;

    private List<GameObject> enemys = new List<GameObject>();
    public List<GameObject> Enemys
    {
        get { return enemys; }
    }

    [SerializeField]
    private Material stageSkyBox = null;
    [SerializeField]
    private AudioClip stageMainBGM = null;
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
    private List<GameObject> spawnObjs = new List<GameObject>();
    [SerializeField]
    private Transform spawnObjTrm = null;

    [SerializeField]
    private Transform playerRespawnTrm = null;
    private Transform PlayerRespawnTrm
    {
        get
        {
            if (playerRespawnTrm == null)
            {
                playerRespawnTrm = GameObject.Find("PlayerRespawnTrm").transform;
            }

            return playerRespawnTrm;
        }
    }

    [SerializeField]
    private float respawnDelay = 2f;
    private float shakeTimer = 0f; // 시네머신을 이용하여 카메라를 흔들 때 사용되는 변수

    private float originOrthographicSize = 0f;

    [SerializeField]
    private CinemachineVirtualCamera cinemachineVirtualCamera = null;
    [SerializeField]
    private CompositeCollider2D cinemachineBoundingShapeCompositeCollider = null;

    private Func<float, float> TimerCheck;

    private void Awake()
    {
        gameManager = GameManager.Instance;
        soundManager = SoundManager.Instance;

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

        EventManager.StartListening("PlayerRespawn", () =>
        {
            gameManager.player.gameObject.SetActive(true);
            gameManager.player.transform.position = PlayerRespawnTrm.position;
        });
    }

    void Start()
    {
        transform.SetAsFirstSibling();
        
        mainCamera = Camera.main;

        cinemachineVirtualCamera = gameManager.cinemachineVirtualCamera;
        cinemachineVirtualCamera.m_Follow = gameManager.player.transform;

        if (stageMainBGM != null)
        {
            soundManager.ChangeMainBGM(stageMainBGM);
        }

        originOrthographicSize = cinemachineVirtualCamera.m_Lens.OrthographicSize;

        if (stageSkyBox != null)
        {
            mainCamera.GetComponent<Skybox>().material = stageSkyBox;
        }

        SetCameraLimitLocation();
    }

    void Update()
    {
        shakeTimer = TimerCheck(shakeTimer);
    }
    public GameObject ShootProjectile(GameObject shootIt, float damage, Vector2 spawnPosition, Quaternion angle, Vector2 shootDir, float shootRange)
    {
        GameObject shootObject = projectiles.Find(x => (x.name == shootIt.name + "(Clone)") && !x.activeSelf);

        if (shootObject == null)
        {
            GameObject a = Instantiate(shootIt, projectilesTrm);
            a.transform.position = spawnPosition;
            a.transform.rotation = angle;
            a.GetComponent<IProjectile>().SpawnSet(shootRange, damage, shootDir);

            shootObject = a;
        }
        else
        {
            shootObject.SetActive(true);
            shootObject.transform.position = spawnPosition;
            shootObject.transform.rotation = angle;
            shootObject.GetComponent<IProjectile>().SpawnSet(shootRange, damage, shootDir);
            projectiles.Remove(shootObject);
        }

        return shootObject;
    }
    public GameObject ShootProjectile(GameObject shootIt, float damage, Vector2 spawnPosition, Quaternion angle, Vector2 shootDir, float shootRange, float alpha)
    {
        GameObject shootObject = projectiles.Find(x => (x.name == shootIt.name + "(Clone)") && !x.activeSelf);

        if (shootObject == null)
        {
            GameObject a = Instantiate(shootIt, projectilesTrm);
            a.transform.position = spawnPosition;
            a.transform.rotation = angle;
            a.GetComponent<IProjectile>().SpawnSet(shootRange, damage, shootDir, alpha);

            shootObject = a;
        }
        else
        {
            shootObject.SetActive(true);
            shootObject.transform.position = spawnPosition;
            shootObject.transform.rotation = angle;
            shootObject.GetComponent<IProjectile>().SpawnSet(shootRange, damage, shootDir, alpha);
            projectiles.Remove(shootObject);
        }

        return shootObject;
    }
    public void DespawnProjectile(GameObject deSpawnProjectile)
    {
        projectiles.Add(deSpawnProjectile);
        deSpawnProjectile.SetActive(false);
    }

    public GameObject SpawnEnemy(GameObject spawnObject, Vector2 spawnPosition)
    {
        GameObject spawnEnemy = enemys.Find(x => (x.name == spawnObject.name + "(Clone)") && (!x.activeSelf));

        if (spawnEnemy == null)
        {
            spawnEnemy = Instantiate(spawnObject, _enemys);
            spawnEnemy.transform.position = spawnPosition;
            enemys.Add(spawnEnemy);
        }
        else
        {
            spawnEnemy.transform.position = spawnPosition;
            spawnEnemy.SetActive(true);
        }

        return spawnEnemy;
    }
    public void DespawnEnemy(GameObject deSpawnObject)
    {
        deSpawnObject.SetActive(false);
    }
    public GameObject SpawnObject(GameObject spawnObject, Vector2 spawnPosition)
    {
        GameObject spawnObj = spawnObjs.Find(x => (x.name == spawnObject.name + "(Clone)"));

        if (spawnObj == null)
        {
            spawnObj = Instantiate(spawnObject, spawnObjTrm);
            spawnObj.transform.position = spawnPosition;
        }
        else
        {
            spawnObj.transform.position = spawnPosition;
            spawnObj.SetActive(true);
            spawnObjs.Remove(spawnObj);
        }

        return spawnObj;
    }
    public void DespawnObject(GameObject deSpawnObject)
    {
        spawnObjs.Add(deSpawnObject);
        deSpawnObject.SetActive(false);
    }
    public GameObject SpawnSoundBox(GameObject spawnIt)
    {
        GameObject spawnSoundBox = soundBoxes.Find(x => (x.name == spawnIt.name + "(Clone)"));

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
    public void SetCameraLimitLocation()
    {
        if (cinemachineBoundingShapeCompositeCollider != null)
        {
            cinemachineVirtualCamera.GetComponent<CinemachineConfiner>().m_BoundingShape2D = cinemachineBoundingShapeCompositeCollider;
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
        Invoke("RespawnPlayer", respawnDelay);
    }
    public void RespawnPlayer()
    {
        EventManager.TriggerEvent("PlayerRespawn");
    }
}
