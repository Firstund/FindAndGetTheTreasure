using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;

public class CharacterDash : MonoBehaviour
{
    private GameManager gameManager = null;
    private StageManager stageManager = null;

    private PlayerInput playerInput = null;
    private CharacterMove characterMove = null;
    private CharacterTimeWarp characterTimeWarp = null;
    private CharacterStat characterStat = null;
    private SpawnAfterImage spawnAfterImage = null;
    private SpawnEffect spawnEffect = null;

    private Rigidbody2D rigid = null;
    private SpriteRenderer spriteRenderer = null;
    private Animator anim = null;

    private string characterName = "";

    [Header("여기부터 이펙트들")]
    [SerializeField]
    private GameObject dashAttackEffect = null;

    [Header("여기부터 사운드박스들")]
    [SerializeField]
    private GameObject dashEffectSoundBox = null;
    [SerializeField]
    private GameObject dashAttackSoundBox = null;

    private Transform GroundChecker = null;
    private Transform LeftWallChecker = null;
    private Transform RightWallChecker = null;
    private Transform UpWallChecker = null;

    [SerializeField]
    private float dashRange = 2f;
    [SerializeField]
    private float dashStopRange = 0.1f;
    [SerializeField]
    private float dashDoTime = 0.1f;
    [SerializeField]
    private float dashResetTime = 1f;

    private Vector2 dashPosition = Vector2.zero;

    private bool canSpawnAfterImageByDash = true;
    private bool canDashAttack = true;

    private void Awake()
    {
        gameManager = GameManager.Instance;
        stageManager = StageManager.Instance;

        playerInput = GetComponent<PlayerInput>();
        characterMove = GetComponent<CharacterMove>();
        characterTimeWarp = GetComponent<CharacterTimeWarp>();
        characterStat = GetComponent<CharacterStat>();
        spawnAfterImage = GetComponent<SpawnAfterImage>();
        spawnEffect = GetComponent<SpawnEffect>();

        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }
    void Start()
    {
        GroundChecker = characterMove.GroundChecker;
        LeftWallChecker = characterMove.LeftWallChecker;
        RightWallChecker = characterMove.RightWallChecker;
        UpWallChecker = characterMove.UpWallChecker;

        characterName = characterStat.characterName;
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
            float XMove = playerInput.XMove;

            if (!gameManager.SlowTimeSomeObjects)
            {
                Dash(XMove);

                DashMove();
                SpawnAfterImageByDash();
            }
        }
    }
    private void Dash(float XMove)
    {
        if (XMove != 0 && characterMove.IsDash && !characterMove.IsHang && !characterMove.Staping && !characterMove.DashMoving && characterMove.CanDash)
        {
            float _dashRange = dashRange;
            dashPosition = GroundChecker.position;

            dashPosition.y += 0.2f;

            if (characterMove.Attacking)
            {
                if (characterStat.sp >= characterMove.SkillUseValue.dashAttack)
                {
                    characterStat.sp -= characterMove.SkillUseValue.dashAttack;
                }
                else
                {
                    characterMove.IsDash = false;
                    return;
                }

                gameManager.SetSlowTime(0.1f);
                _dashRange = DashAttack(_dashRange);
            }
            else
            {
                canDashAttack = false;

                if (characterStat.sp >= characterMove.SkillUseValue.dash)
                {
                    characterStat.sp -= characterMove.SkillUseValue.dash;
                }
                else
                {
                    characterMove.IsDash = false;
                    return;
                }
            }

            if (spriteRenderer.flipX)
            {
                _dashRange = -_dashRange;
            }

            Vector2 endPosition = characterMove.currentPosition;

            endPosition.x = characterMove.currentPosition.x + _dashRange;

            dashPosition = stageManager.PositionCantCrossWall(dashPosition, endPosition, spriteRenderer.flipX, characterMove.WhatIsGround);

            dashPosition.y = characterMove.currentPosition.y;

            stageManager.SpawnSoundBox(dashEffectSoundBox);

            characterMove.DashMoving = true;

            characterMove.IsDash = false;
            characterMove.CanDash = false;

            Invoke("DashRe", dashResetTime);
        }
        else
        {
            characterMove.IsDash = false;
        }

    }
    private float DashAttack(float _dashRange)
    {
        if (canDashAttack)
        {
            characterMove.DashAttacking = true;

            spawnEffect.ShowEffect(dashAttackEffect, Vector2.zero);

            anim.Play(characterName + "DashAttack");

            _dashRange = dashRange * (2f / 3f);

            Vector2 endPosition = characterMove.currentPosition;

            dashPosition = GroundChecker.position;

            dashPosition.y += 0.2f;

            endPosition.x = characterMove.currentPosition.x + _dashRange;

            dashPosition = stageManager.PositionCantCrossWall(dashPosition, endPosition, spriteRenderer.flipX, characterMove.WhatIsGround);

            GetDashAttackDamage();
        }

        return _dashRange;
    }
    private void DashAttackReSet()
    {
        characterMove.DashAttacking = false;
    }

    private void DashRe()
    {
        characterMove.CanDash = true;
        canDashAttack = true;
        characterMove.DashAttacking = false;
    }
    private void SpawnAfterImageByDash()
    {
        if (characterMove.DashMoving && canSpawnAfterImageByDash)
        {
            float spawnAfterImageDelay = UnityEngine.Random.Range(spawnAfterImage.spawnAfterImageDelayMinimum, spawnAfterImage.spawnAfterImageDelayMaximum);
            spawnAfterImage.SetAfterImage();
            canSpawnAfterImageByDash = false;

            Invoke("SpawnAfterImageByDashRe", spawnAfterImageDelay);
        }
    }
    private void SpawnAfterImageByDashRe()
    {
        canSpawnAfterImageByDash = true;
    }
    private void DashMove()
    {
        if (characterMove.DashMoving)
        {
            rigid.velocity = new Vector2(rigid.velocity.x, 0f);
            transform.DOMove(dashPosition, dashDoTime).SetEase(Ease.InQuad);
        }

        Vector2 _dashPosition = dashPosition;
        Vector2 _currentPosition = characterMove.currentPosition;
        _dashPosition.y = 0f;
        _currentPosition.y = 0f;

        float distance = Vector2.Distance(_dashPosition, _currentPosition);

        if (distance <= dashStopRange)
        {
            characterMove.DashMoving = false;
        }
    }
    private void GetDashAttackDamage()
    {
        bool soundPlayed = false;
        float attackRange = dashRange;

        Vector2 _currentPosition = characterMove.currentPosition;

        List<RaycastHit2D> hits = new List<RaycastHit2D>();

        Vector2[] targetPositions = new Vector2[5];

        targetPositions = new Vector2[5] {
            new Vector2(GroundChecker.position.x,UpWallChecker.position.y),
             new Vector2(_currentPosition.x, _currentPosition.y),
          new Vector2(UpWallChecker.position.x, GroundChecker.position.y),
          new Vector2(UpWallChecker.position.x, UpWallChecker.position.y),
          new Vector2(GroundChecker.position.x, GroundChecker.position.y)
           };

        if (spriteRenderer.flipX)
        {
            for (int i = 0; i < 5; i++)
            {
                Vector2 endPosition = new Vector2(targetPositions[i].x - attackRange, targetPositions[i].y);
                targetPositions[i] = stageManager.PositionCantCrossWall(targetPositions[i], endPosition, spriteRenderer.flipX, characterMove.WhatIsGround);
            }
        }
        else
        {
            for (int i = 0; i < 5; i++)
            {
                Vector2 endPosition = new Vector2(targetPositions[i].x + attackRange, targetPositions[i].y);
                targetPositions[i] = stageManager.PositionCantCrossWall(targetPositions[i], endPosition, spriteRenderer.flipX, characterMove.WhatIsGround);
            }
        }

        targetPositions[0] = SetTargetPositionsForRay(GroundChecker.position, targetPositions[0]);
        targetPositions[1] = SetTargetPositionsForRay(_currentPosition, targetPositions[1]);
        targetPositions[2] = SetTargetPositionsForRay(UpWallChecker.position, targetPositions[2]);
        targetPositions[3] = SetTargetPositionsForRay(UpWallChecker.position, targetPositions[3]);
        targetPositions[4] = SetTargetPositionsForRay(GroundChecker.position, targetPositions[4]);

        Ray2D[] rays = new Ray2D[5];

        rays[0] = new Ray2D(GroundChecker.position, targetPositions[0]);
        rays[1] = new Ray2D(_currentPosition, targetPositions[1]);
        rays[2] = new Ray2D(UpWallChecker.position, targetPositions[2]);
        rays[3] = new Ray2D(UpWallChecker.position, targetPositions[3]);
        rays[4] = new Ray2D(GroundChecker.position, targetPositions[4]);

        for (int i = 0; i < 5; i++)
        {
            RaycastHit2D[] hit = Physics2D.RaycastAll(rays[i].origin, rays[i].direction, attackRange, characterMove.WhatIsDashAttackable);

            Debug.DrawRay(rays[i].origin, rays[i].direction, Color.red, 10f);

            hit.ForEach(item =>
            {
                if (characterMove.WhatIsGround != (characterMove.WhatIsGround | 1 << item.transform.gameObject.layer))
                {
                    hits.Add(item);
                }
            });
        }

        hits = hits.Distinct().ToList();

        hits.ForEach(item =>
        {
            EnemyMove enemyMove = item.transform.GetComponent<EnemyMove>();

            if (enemyMove == null)
            {
                BossStat bossStat = item.transform.GetComponent<BossStat>();

                if (bossStat == null)
                {
                    Debug.LogWarning(item.transform.name + " has Neither EnemyMove nor BossStat");
                }
                else
                {
                    if (!bossStat.IsNothurtMode)
                    {
                        bossStat.Hurt(characterStat.ap);

                        if (!soundPlayed)
                        {
                            soundPlayed = true;
                            stageManager.SpawnSoundBox(dashAttackSoundBox);
                        }
                    }
                }
            }
            else
            {
                enemyMove.Hurt(characterStat.ap);

                if (!soundPlayed)
                {
                    soundPlayed = true;
                    stageManager.SpawnSoundBox(dashAttackSoundBox);
                }
            }
        });
    }
    private Vector2 SetTargetPositionsForRay(Vector2 startPosition, Vector2 targetPosition)
    {
        return (targetPosition - startPosition);
    }
}
