using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAttack : MonoBehaviour
{
    private GameManager gameManager = null;
    private StageManager stageManager = null;

    private CharacterMove characterMove = null;
    private CharacterStat characterStat = null;
    private CharacterTimeWarp characterTimeWarp = null;
    private SpawnEffect spawnEffect = null;
    private Reflect reflect = null;


    private Animator anim = null;

    private string characterName = "";

    [Header("여기부터 사운드박스들")]
    [SerializeField]
    private GameObject attackSoundBox = null;
    [Header("여기부터 이펙트들")]
    [SerializeField]
    private GameObject attackEffect = null;

    private void Awake()
    {
        gameManager = GameManager.Instance;
        stageManager = StageManager.Instance;

        characterMove = GetComponent<CharacterMove>();
        characterStat = GetComponent<CharacterStat>();
        characterTimeWarp = GetComponent<CharacterTimeWarp>();
        spawnEffect = GetComponent<SpawnEffect>();
        reflect = GetComponent<Reflect>();

        anim = GetComponent<Animator>();
    }
    private void OnEnable()
    {
        EventManager.StartListening("WhenPlayerInAirToGround", SetAttacking);
    }
    void Start()
    {
        characterName = characterStat.characterName;
    }

    void Update()
    {
        if (characterMove.Attacking)
        {
            DespawnProjectileByAttack();
        }
    }
    private void FixedUpdate()
    {
        if (!gameManager.stopTime)
        {
            if (gameManager.SlowTimeSomeObjects)
            {
                if (gameManager.CurrentSlowTimePerSlowTime == 0)
                {
                    DoFixedUpdate();
                }
            }
            else if (!gameManager.SlowTimeSomeObjects)
            {
                DoFixedUpdate();
            }
        }
    }
    private void DoFixedUpdate()
    {
        if (!(characterMove.IsDead || gameManager.stopTime || characterTimeWarp.isTimeWarp))
        {
            if (!gameManager.SlowTimeSomeObjects)
            {
                Attack();
            }
        }
    }
    private void OnDisable()
    {
        EventManager.StopListening("WhenPlayerInAirToGround", SetAttacking);
    }
    private void Attack()
    {
        if (!characterMove.Attacking && !characterMove.DashMoving && !characterMove.IsHangWall && !characterMove.Staping && characterMove.IsAttack && !characterMove.IsReflect) //isGround에 따라서 GroundAttack과 InAirAttack을 나눌것, dashing == true라면 dashAttack을 할것
        {
            if (characterStat.sp >= characterMove.SkillUseValue.attack)
            {
                characterStat.sp -= characterMove.SkillUseValue.attack;
            }
            else
            {
                characterMove.IsAttack = false;
                return;
            }

            spawnEffect.ShowEffect(attackEffect, Vector2.zero);
            if (characterMove.IsGround)
            {
                characterMove.Attacking = true;

                anim.Play(characterName + "Attack");

                characterMove.IsAttack = false;
            }
            else
            {
                characterMove.Attacking = true;

                anim.Play(characterName + "InAirAttack");

                characterMove.IsAttack = false;
            }

            GetDamage();
        }
    }
    private void GetDamage()
    {
        bool soundPlayed = false;
        Collider2D[] a = Physics2D.OverlapCircleAll(characterMove.currentPosition, characterStat.attackRange, characterMove.WhatIsEnemy);

        a.ForEach(item =>
        {
            EnemyMove enemyMove = item.GetComponent<EnemyMove>();

            if (enemyMove == null)
            {
                BossStat bossStat = item.GetComponent<BossStat>();

                if (bossStat == null)
                {
                    Debug.LogWarning(item.name + " has Neither EnemyMove nor BossStat");
                }
                else
                {
                    if (!bossStat.IsNothurtMode)
                    {
                        bossStat.Hurt(characterStat.ap);

                        if (!soundPlayed)
                        {
                            stageManager.SpawnSoundBox(attackSoundBox);
                            soundPlayed = true;
                        }
                    }
                }
            }
            else
            {
                enemyMove.Hurt(characterStat.ap);

                if (!soundPlayed)
                {
                    stageManager.SpawnSoundBox(attackSoundBox);
                    soundPlayed = true;
                }
            }
        });
    }
    private void DespawnProjectileByAttack()
    {
        if (characterMove.SkillUseValue.reflect < characterStat.sp && !characterMove.IsReflect)
        {
            bool projectileDespawned = false;
            float distance;
            float damage = 0f;

            for (int i = 0; i < stageManager.ProjectilesTrm.childCount; i++)
            {
                GameObject item = stageManager.ProjectilesTrm.GetChild(i).gameObject;

                if (item.activeSelf)
                {
                    distance = Vector2.Distance(characterMove.currentPosition, item.transform.position);

                    if (distance <= characterStat.attackRange)
                    {
                        stageManager.DespawnProjectile(item);
                        damage += 2;
                        projectileDespawned = true;
                    }
                }
            }

            if (projectileDespawned)
            {
                reflect.CanSettingAngle = true;
                reflect.CanShoot = true;
                characterMove.Attacking = false;
                characterMove.IsReflect = true;

                reflect.ProjectileDamage = damage;
                characterStat.sp -= characterMove.SkillUseValue.reflect;
            }
        }
    }
    private void SetAttacking()
    {
        characterMove.Attacking = false;
        characterMove.IsAttack = false;
    }
}
