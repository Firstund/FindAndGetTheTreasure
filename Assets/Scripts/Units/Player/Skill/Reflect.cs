using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reflect : MonoBehaviour
{
    private GameManager gameManager = null;
    private StageManager stageManager = null;
    private SoundManager soundManager = null;

    private bool isAttack = false;
    private bool timeSlowSoundEffectBoxSpawned = false;
    private bool canReflect = true;

    private bool canShoot = false;
    public bool CanShoot
    {
        get { return canShoot; }
        set
        {
            if (canReflect)
            {
                canShoot = value;
            }
        }
    }
    private bool canSettingAngle = false;
    public bool CanSettingAngle
    {
        get
        {
            return canSettingAngle;
        }
        set
        {
            if (canReflect)
            {
                canSettingAngle = value;
                // soundManager.SetMainBGMPitchByLerp(1, -5, refelctTimer.GetComponent<ReflectTimer>().getTime);
            }
        }
    }

    [SerializeField]
    private float upDownSpeed = 1f;
    [SerializeField]
    private float projectileShootRange = 20f;

    [SerializeField]
    private GameObject projectile = null;
    [SerializeField]
    private GameObject arrowAtEnd = null;
    [SerializeField]
    private GameObject refelctTimer = null;
    [SerializeField]
    private GameObject timeSlowEffectObj = null;

    [SerializeField]
    private GameObject timeSlowEffectSoundBox = null;
    [SerializeField]
    private GameObject timeSlowEffectSoundBox2 = null;
    private GameObject currentEffectSoundBox = null;

    [SerializeField]
    private LineRenderer projectileShootLine = null;
    [SerializeField]
    private Animator anim = null;
    private PlayerInput playerInput = null;

    [SerializeField]
    private Transform shootTrm = null;
    private Vector2 shootAngle = Vector2.zero;

    private float shootAnlgePlus = 0f;
    private float projectileDamage = 0f;
    public float ProjectileDamage
    {
        get { return projectileDamage; }
        set { projectileDamage = value; }
    }

    private void Awake()
    {
        gameManager = GameManager.Instance;
        soundManager = SoundManager.Instance;
    }
    void Start()
    {
        stageManager = StageManager.Instance;

        playerInput = GetComponent<PlayerInput>();

        anim = GetComponent<Animator>();


        // canSettingAngle = true;
        // canShoot = true;
    }
    private void OnEnable()
    {
        EventManager.StartListening("StopSlowTimeByLerp", WhenTimeNotSlow);

        EventManager.StartListening("SetFalseSlowTimeSomeObjects", WhenTimeNotSlow);
    }
    private void OnDisable()
    {
        EventManager.StopListening("StopSlowTimeByLerp", WhenTimeNotSlow);

        EventManager.StopListening("SetFalseSlowTimeSomeObjects", WhenTimeNotSlow);
    }

    private void WhenTimeNotSlow()
    {
        isAttack = false;
        CanShoot = false;
        CanSettingAngle = false;
        timeSlowSoundEffectBoxSpawned = false;

        shootAnlgePlus = 0f;

        if (currentEffectSoundBox != null)
        {
            stageManager.DesapwnSoundBox(currentEffectSoundBox);
        }

        for (int i = 0; i < 2; i++)
        {
            if (projectileShootLine != null)
            {
                projectileShootLine.SetPosition(i, Vector2.zero);
            }
        }
    }

    void Update()
    {
        if (CanShoot && playerInput.isAttack)
        {
            isAttack = true;
        }

        projectileShootLine.gameObject.SetActive(CanSettingAngle);
        arrowAtEnd.SetActive(CanSettingAngle);
    }
    void FixedUpdate()
    {
        shootAngle = transform.position;

        if (CanSettingAngle)
        {
            SettingAngle();
        }
        if (CanShoot)
        {
            gameManager.SlowTimeSomeObjects = true;
            Shoot();
        }
    }
    public void Shoot() // 공격을 통해 파괴한 투사체가 많을수록 강한 데미지
    {
        if (isAttack)
        {
            //Instantiate(projectile, shootTrm.position, Quaternion.Euler(0f, 0f, shootAnlgePlus)).GetComponent<PlayerProjectile>().SpawnSet(projectileShootRange, projectileDamage, Vector2.right);
            stageManager.ShootProjectile(projectile, projectileDamage, shootTrm.position, Quaternion.Euler(0f, 0f, shootAnlgePlus), Vector2.right, projectileShootRange);
            WhenReflectEnd();
        }
    }

    public void WhenReflectEnd()
    {
        SpawnDespawnEffects(false);

        shootTrm.rotation = Quaternion.identity;

        gameManager.player.IsReflect = false;
        gameManager.SlowTimeSomeObjects = false;
        playerInput.isAttack = false;
        isAttack = false;
    }

    public void SettingAngle()
    {
        if (!timeSlowSoundEffectBoxSpawned)
        {
            stageManager.SpawnSoundBox(timeSlowEffectSoundBox);
            currentEffectSoundBox = stageManager.SpawnSoundBox(timeSlowEffectSoundBox2);

            SpawnDespawnEffects(true);

            anim.Play(gameManager.player.CharacterStat.characterName + "ReflectR");

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

    public void SpawnDespawnEffects(bool spawn)
    {
        try
        {
            refelctTimer.SetActive(spawn);
            timeSlowEffectObj.SetActive(spawn);
        }
        catch (Exception)
        {

        }
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
