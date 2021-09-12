using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reflect : MonoBehaviour
{
    private GameManager gameManager = null;
    private StageManager stageManager = null;

    private bool isAttack = false;
    private bool timeSlowSoundEffectBoxSpawned = false;
    public bool canShoot { private get; set; }
    public bool canSettingAngle { private get; set; }

    [SerializeField]
    private float upDownSpeed = 1f;
    [SerializeField]
    private float maxPosY = 5f;

    [SerializeField]
    private GameObject projectile = null;
    [SerializeField]
    private GameObject arrowAtEnd = null;
    [SerializeField]
    private GameObject timeSlowSoundEffectBox = null;
    private GameObject currentSoundEffectBox = null;
    [SerializeField]
    private LineRenderer projectileShootLine = null;
    private PlayerInput playerInput = null;

    [SerializeField]
    private Transform shootTrm = null;
    private Vector2 shootAngle = Vector2.zero;
    private float shootAnlgePlus = 0f;

    private void Awake()
    {
        gameManager = GameManager.Instance;
    }
    void Start()
    {
        stageManager = FindObjectOfType<StageManager>();

        playerInput = GetComponent<PlayerInput>();

        canSettingAngle = true;
        canShoot = true;
    }
    private void OnEnable()
    {
        gameManager.StopSlowTimeByLerp += () =>
        {
            WhenTimeNotSlow();
        };

        gameManager.SetFalseSlowTimeSomeObjects += () =>
        {
            WhenTimeNotSlow();
        };
    }
    private void OnDisable()
    {
        gameManager.StopSlowTimeByLerp -= () =>
        {
            WhenTimeNotSlow();
        };

        gameManager.SetFalseSlowTimeSomeObjects -= () =>
        {
            WhenTimeNotSlow();
        };
    }

    private void WhenTimeNotSlow()
    {
        isAttack = false;
        canShoot = false;
        canSettingAngle = false;
        timeSlowSoundEffectBoxSpawned = false;
        
        shootAnlgePlus = 0f;

        stageManager.DesapwnSoundBox(currentSoundEffectBox);

        for (int i = 0; i < 2; i++)
        {
            projectileShootLine.SetPosition(i, Vector2.zero);
        }
    }

    void Update()
    {
        if (canShoot && playerInput.isAttack)
        {
            isAttack = true;
        }

        projectileShootLine.gameObject.SetActive(canSettingAngle);
        arrowAtEnd.SetActive(canSettingAngle);
    }
    void FixedUpdate()
    {
        shootAngle = transform.position;

        if (canSettingAngle)
        {
            SettingAngle();
        }
        if (canShoot)
        {
            gameManager.SlowTimeSomeObjects = true;
            Shoot();
        }
    }
    public void Shoot()
    {
        if (isAttack)
        {
            Instantiate(projectile, shootTrm.position, Quaternion.Euler(0f, 0f, shootAnlgePlus)).GetComponent<PlayerProjectile>().SpawnSet(10, 1, Vector2.right);

            gameManager.SlowTimeSomeObjects = false;
        }
    }
    public void SettingAngle()
    {
        if(!timeSlowSoundEffectBoxSpawned)
        {
            currentSoundEffectBox = stageManager.SpawnSoundBox(timeSlowSoundEffectBox);
            timeSlowSoundEffectBoxSpawned = true;
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            shootAnlgePlus += upDownSpeed;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            shootAnlgePlus -= upDownSpeed;
        }

        shootTrm.rotation = Quaternion.Euler(0f, 0f, shootAnlgePlus);

        projectileShootLine.SetPosition(0, (Vector2)shootTrm.localPosition);
        projectileShootLine.SetPosition(1, shootTrm.right);

        arrowAtEnd.transform.localPosition = shootTrm.right;
        arrowAtEnd.transform.rotation = Quaternion.Euler(0f, 0f, shootAnlgePlus);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Ray2D ray = new Ray2D();
        ray.origin = (Vector2)shootTrm.position;
        ray.direction = (shootAngle - (Vector2)shootTrm.position);

        Gizmos.DrawRay(ray.origin, ray.direction);
    }
}
