using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;
using System;

[Serializable]
public struct SkillUseSpValue
{
    public float attack;
    public float reflect;
    public float dash;
    public float dashAttack;
    public float timeWarp;
}
public class CharacterMove : MonoBehaviour
{
    private GameManager gameManager = null;
    private StageManager stageManager = null;

    private string characterName;
    private Rigidbody2D rigid = null;
    public Rigidbody2D Rigid
    {
        get { return rigid; }
    }
    private BoxCollider2D boxCol2D = null;
    private Animator anim = null;
    public SpriteRenderer spriteRenderer { get; private set; }
    private SpriteRenderer pulleySpriteRenderer = null;
    private SpawnEffect spawnEffect = null;

    private PlayerInput playerInput = null;
    private SpawnAfterImage spawnAfterImage = null;
    private CharacterStat characterStat = null;
    public CharacterStat CharacterStat
    {
        get { return characterStat; }
    }
    private Reflect reflect = null;

    private CharacterTimeWarp characterTimeWarp = null;

    [SerializeField]
    private GameObject pulley = null;
    [Header("여기부터 사운드박스들")]
    [SerializeField]
    private GameObject dashAttackSoundBox = null;
    [SerializeField]
    private GameObject attackSoundBox = null;
    [SerializeField]
    private GameObject hurtSoundBox = null;
    [SerializeField]
    private GameObject jumpSoundBox = null;
    [SerializeField]
    private GameObject stapSoundBox = null;

    [Header("여기부터 이펙트들")]
    [SerializeField]
    private GameObject attackEffect = null;
    [SerializeField]
    private GameObject dashAttackEffect = null;
    [SerializeField]
    private GameObject slideAtSideWall = null;
    [SerializeField]
    private GameObject jumpEffect = null;
    [SerializeField]
    private GameObject stapEffect = null;
    [SerializeField]
    private GameObject reflectEffect = null;
    private GameObject currentSlideAtSideWallEffect = null;

    [Header("스킬을 사용할 때 소모되는 SP의 양")]
    [SerializeField]
    private SkillUseSpValue skillUseValue;
    public SkillUseSpValue SkillUseSpValue
    {
        get { return skillUseValue; }
    }

    [SerializeField]
    private LayerMask whatIsGround;
    [SerializeField]
    private LayerMask whatIsEnemy;
    [SerializeField]
    private LayerMask whatIsHitable;
    [SerializeField]
    private LayerMask whatIsPorjectile;

    [SerializeField]
    private Transform GroundChecker;
    [SerializeField]
    private Transform LeftWallChecker;
    [SerializeField]
    private Transform RightWallChecker;
    [SerializeField]
    private Transform UpWallChecker;

    private bool isGround = false;
    private bool leftWall = false;
    private bool rightWall = false;
    private bool upWall = false;

    [SerializeField]
    private float dashRange = 2f;
    [SerializeField]
    private float dashStopRange = 0.1f;
    [SerializeField]
    private float dashDoTime = 0.1f;
    [SerializeField]
    private float dashResetTime = 1f;

    [SerializeField]
    private float powerTimeWhenIsHangFalse = 5f;
    private float powerTimer = 0f;
    private bool isJump = false;
    private bool isHang = false;
    private bool IsHang
    {
        get
        {
            return isHang;
        }
        set
        {
            if (value)
            {
                powerTimer = 0f;
            }
            else if (!gameManager.SlowTimeSomeObjects)
            {
                giveLeftPowerWhenIsHangFalse = spriteRenderer.flipX;

                WhenIsHangFalse();
            }
            else
            {
                powerTimer = 0f;
            }

            isHang = value;
        }
    }
    private bool giveLeftPowerWhenIsHangFalse = false;
    private bool isDash = false;
    private bool isDead = false;
    private bool isAttack = false;
    private bool isHurt = false;
    private bool _canHurt = true;
    public bool canHurt
    {
        get { return _canHurt; }
    }
    private bool isHangWall = false;
    private bool isReflect = false;
    public bool IsReflect
    {
        get { return isReflect; }
        set { isReflect = value; }
    }
    private bool canDash = true;
    private bool canDashAttack = true;
    private bool dashMoving = false;
    private bool staping = false;
    private bool attacking = false;
    public bool Attacking
    {
        get { return attacking; }
        set { attacking = value; }
    }
    private bool dashAttacking = false;
    public bool DashAttacking
    {
        get { return dashAttacking; }
    }
    private bool canJumpAgain = false;

    private bool canSpawnAfterImageByDash = true;

    [Header("떨어지기 시작했을 때 이 값만큼 아래로 이동하면, 낙사처리")]
    [SerializeField]
    private float dropValue = 5f;
    private Vector2 whenIsNotInAirPosition = Vector2.zero; // 공중에 떨어져 있지 않을 때의 위치

    private Vector2 dashPosition = Vector2.zero;

    public Vector2 currentPosition { get; private set; }

    private float firstGravity = 0f;
    private float firstMass = 0f;

    public event Action WhenPlayerDead;
    public event Action WhenPlayerInAir;
    public event Action WhenInAirToGround;

    // TODO: OnCollision을 이용하여 벽에 붙었을 때 서서히 내려감

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        boxCol2D = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        pulleySpriteRenderer = pulley.GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        playerInput = GetComponent<PlayerInput>();
        characterStat = GetComponent<CharacterStat>();
        spawnAfterImage = GetComponent<SpawnAfterImage>();
        characterTimeWarp = GetComponent<CharacterTimeWarp>();
        spawnEffect = GetComponent<SpawnEffect>();
        reflect = GetComponent<Reflect>();

        WhenPlayerDead = () =>
        {

        };

        WhenPlayerInAir = () =>
        {

        };

        WhenInAirToGround = () =>
        {

        };
    }

    void Start()
    {
        gameManager = GameManager.Instance;
        stageManager = StageManager.Instance;

        stageManager.SetPlayerRespawnPosition(transform.position);

        stageManager.PlayerRespawn += () =>
        {
            whenIsNotInAirPosition = currentPosition;
            characterStat.hp = characterStat.firstHp;
        };

        if (playerInput == null)
        {
            playerInput = gameObject.AddComponent<PlayerInput>();
        }

        characterName = characterStat.characterName;
        firstGravity = rigid.gravityScale;
        firstMass = rigid.mass;

        whatIsHitable.value = whatIsEnemy + whatIsGround;

        characterStat.hp = characterStat.firstHp;

        currentPosition = transform.position;
        whenIsNotInAirPosition = currentPosition;

        pulley.SetActive(false);
    }
    void Update()
    {
        if (!(isDead || gameManager.stopTime || characterTimeWarp.isTimeWarp || isReflect))
        {
            if (anim.speed == 0f)
            {
                anim.speed = 1f;
            }

            CheckStatus();

            GroundCheck();
            UpWallCheck();
            LeftWallCheck();
            RightWallCheck();
            CharacterHangWallCheck();
            IsHangFalseGivePower();
        }
        else if (gameManager.stopTime && !characterTimeWarp.isTimeWarp && isReflect)
        {
            anim.speed = 0f;
        }

        SetAnimByTimeWarp();
        CheckDead();

        if (attacking)
        {
            DespawnProjectileByAttack();
        }
    }
    private void OnEnable()
    {
        isDead = false;
        attacking = false;

        spriteRenderer.color = new Vector4(1f, 1f, 1f, 1f);
    }
    private void WhenIsHangFalse()
    {
        powerTimer = powerTimeWhenIsHangFalse;
    }
    private void IsHangFalseGivePower()
    {
        if (powerTimer > 0f)
        {
            if (giveLeftPowerWhenIsHangFalse)
            {
                // rigid.velocity = new Vector2(Mathf.Lerp(0f, 1f, powerTimer / powerTimeWhenIsHangFalse), rigid.velocity.y);
                rigid.AddForce(new Vector2(Mathf.Lerp(0f, 1f, powerTimer / powerTimeWhenIsHangFalse), 0f));
            }
            else
            {
                // rigid.velocity = new Vector2(Mathf.Lerp(0f, -1f, powerTimer / powerTimeWhenIsHangFalse), rigid.velocity.y);
                rigid.AddForce(new Vector2(Mathf.Lerp(0f, -1f, powerTimer / powerTimeWhenIsHangFalse), 0f));
            }

            powerTimer -= Time.deltaTime;
        }
        else
        {
            powerTimer = 0f;
        }
    }
    private void CheckDead()
    {
        if (!isDead && !gameManager.stopTime)
        {
            if (!isGround && whenIsNotInAirPosition.y - currentPosition.y >= dropValue)
            {
                Hurt(100);
            }

            if (characterStat.hp <= 0f)
            {
                isDead = true;
                isJump = false;
                isDash = false;
                isAttack = false;
                isGround = false;
                attacking = false;

                Dead();
            }
        }
    }

    private void SetAnimByTimeWarp()
    {
        if (characterTimeWarp.isTimeWarp)
        {
            anim.enabled = false;
        }
        else
        {
            anim.enabled = true;
        }
    }

    private void CheckStatus()
    {
        if (playerInput.isJump && !staping)
        {
            attacking = false;
            if (upWall && !(isGround || leftWall || rightWall))
            {
                if (!IsHang)
                {
                    IsHang = true;

                    whenIsNotInAirPosition = currentPosition;
                    isJump = false;
                }
                else
                {
                    IsHang = false;
                }
            }
            else
            {
                isJump = true;
                if (IsHang)
                {
                    IsHang = false;
                }
            }
        }

        if (!upWall)
        {
            if (IsHang)
            {
                IsHang = false;
            }
        }

        if (playerInput.isDash)
        {
            isDash = true;
        }

        if (attacking)
        {
            playerInput.isAttack = false;
        }

        if (playerInput.isAttack)
        {
            isAttack = true;
        }
    }

    void FixedUpdate()
    {
        currentPosition = transform.position;

        if (gameManager.stopTime)
        {
            rigid.gravityScale = 0f;
            rigid.mass = 0f;
            rigid.velocity = Vector2.zero;
        }
        else
        {
            rigid.gravityScale = firstGravity;
            rigid.mass = firstMass;
        }

        if (!gameManager.stopTime)
        {
            if (gameManager.SlowTimeSomeObjects)
            {
                anim.speed = 1f / gameManager.SlowTimeNum;

                rigid.gravityScale = firstGravity / gameManager.SlowTimeNum;
                rigid.mass = firstMass / gameManager.SlowTimeNum;

                rigid.velocity = Vector2.zero;

                if (gameManager.CurrentSlowTimePerSlowTime == 0)
                {
                    DoFixedUpdate();
                }
            }
            else if (!gameManager.SlowTimeSomeObjects)
            {
                DoFixedUpdate();

                anim.speed = 1f;

                if (!IsHang)
                {
                    rigid.gravityScale = firstGravity;
                }

                rigid.mass = firstMass;
            }
        }



        transform.position = currentPosition;
    }

    private void DoFixedUpdate()
    {
        if (!(isDead || gameManager.stopTime || characterTimeWarp.isTimeWarp))
        {
            float XMove = playerInput.XMove;

            if (gameManager.SlowTimeSomeObjects)
            {
                XMove /= gameManager.SlowTimeNum;
            }

            LRCheck(XMove);

            if (gameManager.SlowTimeSomeObjects)
            {
                isJump = false;
                IsHang = false;
                isAttack = false;
                attacking = false;

                XMove = 0f;

                anim.Play(name + "ReflectR");

                reflectEffect.SetActive(true);
                reflectEffect.transform.position = spriteRenderer.flipX ? RightWallChecker.position : LeftWallChecker.position;
            }
            else
            {
                Jump();
                Hang();
                Attack();
                MoveX(XMove);

                InAirCheck();
                Dash(XMove);

                DashMove();
                SpawnAfterImageByDash();

                reflectEffect.SetActive(false);
            }
        }
    }

    private void OnCollisionStay2D(Collision2D col)
    {
        int layer = 2 << col.gameObject.layer - 1;

        if (layer == LayerMask.GetMask("GROUND"))
        {
            if (rigid.velocity.y < 0f)
            {
                // 벽에서 미끄러 떨어지는 파티클 추가

                if (currentSlideAtSideWallEffect == null)
                {
                    if (spriteRenderer.flipX)
                    {
                        spawnEffect.ShowEffect(slideAtSideWall, LeftWallChecker.position);
                    }
                    else
                    {
                        spawnEffect.ShowEffect(slideAtSideWall, RightWallChecker.position);
                    }
                }
            }
        }
    }
    private void Hang()
    {
        if (IsHang)
        {
            pulleySpriteRenderer.flipX = spriteRenderer.flipX;
            pulley.SetActive(true);

            rigid.gravityScale = -1f;

            anim.Play(characterName + "Hang");
            canJumpAgain = true;
        }
        else
        {
            rigid.gravityScale = firstGravity;

            pulley.SetActive(false);
        }
    }
    public void Hurt(float damage)
    {
        if (canHurt && !isHurt && canHurt)
        {
            isHurt = true;
            _canHurt = false;
            stageManager.ShakeCamera(2f, 0.1f);
            stageManager.SpawnSoundBox(hurtSoundBox);

            float hp = characterStat.hp;
            float dp = characterStat.dp;

            float totalDamage;

            totalDamage = damage - dp;

            if (totalDamage <= 0f)
            {
                totalDamage = 0.5f;
            }

            hp -= totalDamage;
            characterStat.hp = hp;

            StartCoroutine(hurt());

            Invoke("isHurtSet", 1f);

        }
    }
    private IEnumerator hurt()
    {
        Color color = new Color(1f, 0f, 1f, 0.5f);
        Color color_origin = new Color(1f, 1f, 1f, 1f);

        spriteRenderer.color = color;

        // HitSound 재생

        yield return new WaitForSeconds(0.5f);

        spriteRenderer.color = color_origin;
    }
    private void isHurtSet()
    {
        isHurt = false;
        _canHurt = true;
    }
    private void Dead()
    {
        anim.Play(characterName + "Dead");
    }
    private void Destroye()
    {
        WhenPlayerDead();

        stageManager.Respawn();
        gameObject.SetActive(false);
    }
    private void Dash(float XMove)
    {
        if (XMove != 0 && isDash && !IsHang && !staping && !dashMoving && canDash)
        {
            float _dashRange = dashRange;
            dashPosition = GroundChecker.position;

            dashPosition.y += 0.2f;

            if (attacking)
            {
                if (characterStat.sp >= skillUseValue.dashAttack)
                {
                    characterStat.sp -= skillUseValue.dashAttack;
                }
                else
                {
                    isDash = false;
                    return;
                }

                gameManager.SetSlowTime(0.1f);
                _dashRange = DashAttack(_dashRange);
            }
            else
            {
                canDashAttack = false;

                if (characterStat.sp >= skillUseValue.dash)
                {
                    characterStat.sp -= skillUseValue.dash;
                }
                else
                {
                    isDash = false;
                    return;
                }
            }

            if (spriteRenderer.flipX)
            {
                _dashRange = -_dashRange;
            }

            Vector2 endPosition = currentPosition;

            endPosition.x = currentPosition.x + _dashRange;

            dashPosition = stageManager.PositionCantCrossWall(dashPosition, endPosition, spriteRenderer.flipX, whatIsGround);

            dashPosition.y = currentPosition.y;

            dashMoving = true;

            isDash = false;
            canDash = false;

            Invoke("DashRe", dashResetTime);
        }
        else
        {
            isDash = false;
        }

    }
    private float DashAttack(float _dashRange)
    {
        if (canDashAttack)
        {
            dashAttacking = true;

            spawnEffect.ShowEffect(dashAttackEffect, Vector2.zero);

            anim.Play(characterName + "DashAttack");

            _dashRange = dashRange * (2f / 3f);

            Vector2 endPosition = currentPosition;

            dashPosition = GroundChecker.position;

            dashPosition.y += 0.2f;

            endPosition.x = currentPosition.x + _dashRange;

            dashPosition = stageManager.PositionCantCrossWall(dashPosition, endPosition, spriteRenderer.flipX, whatIsGround);

            GetDashAttackDamage();
        }

        return _dashRange;
    }
    private void DashAttackReSet()
    {
        dashAttacking = false;
    }

    private void DashRe()
    {
        canDash = true;
        canDashAttack = true;
        dashAttacking = false;
    }
    private void SpawnAfterImageByDash()
    {
        if (dashMoving && canSpawnAfterImageByDash)
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
        if (dashMoving)
        {
            rigid.velocity = new Vector2(rigid.velocity.x, 0f);
            transform.DOMove(dashPosition, dashDoTime).SetEase(Ease.InQuad);
        }

        Vector2 _dashPosition = dashPosition;
        Vector2 _currentPosition = currentPosition;
        _dashPosition.y = 0f;
        _currentPosition.y = 0f;

        float distance = Vector2.Distance(_dashPosition, _currentPosition);

        if (distance <= dashStopRange)
        {
            dashMoving = false;
        }
    }
    private void Attack()
    {
        if (!attacking && !dashMoving && !isHangWall && !staping && isAttack && !isReflect) //isGround에 따라서 GroundAttack과 InAirAttack을 나눌것, dashing == true라면 dashAttack을 할것
        {
            if (characterStat.sp >= skillUseValue.attack)
            {
                characterStat.sp -= skillUseValue.attack;
            }
            else
            {
                isAttack = false;
                return;
            }

            spawnEffect.ShowEffect(attackEffect, Vector2.zero);
            if (isGround)
            {
                attacking = true;

                anim.Play(characterName + "Attack");

                isAttack = false;
            }
            else
            {
                attacking = true;

                anim.Play(characterName + "InAirAttack");

                isAttack = false;
            }

            GetDamage();
        }
    }
    private void GetDamage()
    {
        bool soundPlayed = false;
        Collider2D[] a = Physics2D.OverlapCircleAll(currentPosition, characterStat.attackRange, whatIsEnemy);

        a.ForEach(item =>
        {
            if (!soundPlayed)
            {
                stageManager.SpawnSoundBox(attackSoundBox);
                soundPlayed = true;
            }

            EnemyMove enemyMove = item.GetComponent<EnemyMove>();

            enemyMove.Hurt(characterStat.ap);
        });
    }
    private void DespawnProjectileByAttack()
    {
        if (skillUseValue.reflect < characterStat.sp && !isReflect)
        {
            bool projectileDespawned = false;
            float distance;
            float damage = 0f;

            for (int i = 0; i < stageManager.ProjectilesTrm.childCount; i++)
            {
                GameObject item = stageManager.ProjectilesTrm.GetChild(i).gameObject;

                if (item.activeSelf)
                {
                    distance = Vector2.Distance(currentPosition, item.transform.position);

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
                attacking = false;
                isReflect = true;

                reflect.ProjectileDamage = damage;
                characterStat.sp -= skillUseValue.reflect;
            }
        }
    }
    private void GetDashAttackDamage() // 판정관련 오류 있음
    {
        bool soundPlayed = false;
        float attackRange = dashRange;

        Vector2 _currentPosition = currentPosition;

        List<RaycastHit2D> hits = new List<RaycastHit2D>();

        Vector2[] targetPositions = new Vector2[5];

        targetPositions = new Vector2[5] {
            new Vector2(GroundChecker.position.x,UpWallChecker.position.y),
             new Vector2(currentPosition.x, currentPosition.y),
          new Vector2(UpWallChecker.position.x, GroundChecker.position.y),
          new Vector2(UpWallChecker.position.x, UpWallChecker.position.y),
          new Vector2(GroundChecker.position.x, GroundChecker.position.y)
           };

        if (spriteRenderer.flipX)
        {
            for (int i = 0; i < 5; i++)
            {
                Vector2 endPosition = new Vector2(targetPositions[i].x - attackRange, targetPositions[i].y);
                targetPositions[i] = stageManager.PositionCantCrossWall(targetPositions[i], endPosition, spriteRenderer.flipX, whatIsGround);
            }
        }
        else
        {
            for (int i = 0; i < 5; i++)
            {
                Vector2 endPosition = new Vector2(targetPositions[i].x + attackRange, targetPositions[i].y);
                targetPositions[i] = stageManager.PositionCantCrossWall(targetPositions[i], endPosition, spriteRenderer.flipX, whatIsGround);
            }
        }

        targetPositions[0] = SetTargetPositionsForRay(GroundChecker.position, targetPositions[0]);
        targetPositions[1] = SetTargetPositionsForRay(currentPosition, targetPositions[1]);
        targetPositions[2] = SetTargetPositionsForRay(UpWallChecker.position, targetPositions[2]);
        targetPositions[3] = SetTargetPositionsForRay(UpWallChecker.position, targetPositions[3]);
        targetPositions[4] = SetTargetPositionsForRay(GroundChecker.position, targetPositions[4]);

        Ray2D[] rays = new Ray2D[5];

        rays[0] = new Ray2D(GroundChecker.position, targetPositions[0]);
        rays[1] = new Ray2D(currentPosition, targetPositions[1]);
        rays[2] = new Ray2D(UpWallChecker.position, targetPositions[2]);
        rays[3] = new Ray2D(UpWallChecker.position, targetPositions[3]);
        rays[4] = new Ray2D(GroundChecker.position, targetPositions[4]);

        for (int i = 0; i < 5; i++)
        {
            RaycastHit2D[] hit = Physics2D.RaycastAll(rays[i].origin, rays[i].direction, attackRange, whatIsHitable);

            Debug.DrawRay(rays[i].origin, rays[i].direction, Color.red, 10f);

            hit.ForEach(item =>
            {
                if (whatIsGround != (whatIsGround | 1 << item.transform.gameObject.layer))
                {
                    hits.Add(item);
                }
            });
        }

        hits = hits.Distinct().ToList();

        hits.ForEach(item =>
        {
            EnemyMove enemyMove = item.transform.GetComponent<EnemyMove>();

            if (enemyMove != null)
            {
                if (!soundPlayed)
                {
                    soundPlayed = true;
                    stageManager.SpawnSoundBox(dashAttackSoundBox);
                }

                enemyMove.Hurt(characterStat.ap);
            }
        });
    }
    private Vector2 SetTargetPositionsForRay(Vector2 startPosition, Vector2 targetPosition)
    {
        return (targetPosition - startPosition);
    }
    private void SetAttacking()
    {
        attacking = false;
        isAttack = false;
    }
    private void Jump()
    {
        if ((isJump && !staping && !attacking && isGround && !isHangWall) || (canJumpAgain && isJump && !staping && !attacking && !isHangWall))
        {
            if (canJumpAgain)
            {
                rigid.velocity = new Vector2(rigid.velocity.x, 0f);
                canJumpAgain = false;
            }

            if (isGround)
            {
                canJumpAgain = true;
            }

            attacking = false;

            anim.Play(characterName + "Jump");
        }
    }
    public void Jumping()
    {
        rigid.velocity = new Vector2(rigid.velocity.x, characterStat.jumpSpeed);

        isJump = false;
        attacking = false;

        spawnEffect.ShowEffect(jumpEffect, transform.position);
        stageManager.SpawnSoundBox(jumpSoundBox);
    }
    private void InAirCheck()
    {
        if (!(isJump || isGround || IsHang || staping || attacking || isReflect))
        {
            if (isHangWall)
            {
                spriteRenderer.flipX = leftWall;

                anim.Play(characterName + "HangWall");
            }
            else
            {
                anim.Play(characterName + "InAir");
            }
        }
    }

    private void MoveX(float XMove)
    {
        if (!IsHang && powerTimer <= 0f)
        {
            rigid.velocity = new Vector2(XMove * characterStat.speed, rigid.velocity.y);
        }
        else if (IsHang)
        {
            if (spriteRenderer.flipX)
            {
                rigid.velocity = new Vector2(-characterStat.hangSpeed, rigid.velocity.y);
            }
            else
            {
                rigid.velocity = new Vector2(characterStat.hangSpeed, rigid.velocity.y);
            }
        }
    }

    private void GroundCheck()
    {
        bool a = Physics2D.OverlapCircle(GroundChecker.position, 0.05f, whatIsGround);

        if (!isGround && a) // 착지하는 순간
        {
            SetAttacking();
            isJump = false;
            staping = true;
            attacking = false;
            anim.Play(characterName + "Stap");

            Vector2 spawnPos = transform.position;
            spawnPos.y--;

            spawnEffect.ShowEffect(stapEffect, spawnPos);
            stageManager.SpawnSoundBox(stapSoundBox);

            if (rigid.velocity.y <= -20f)
            {
                if (canHurt)
                {
                    Hurt(0.1f * (-(rigid.velocity.y + 20f)) + characterStat.dp);
                }
            }

            WhenInAirToGround();

            canJumpAgain = false;
        }

        if (isGround && !a)// 공중으로 떨어진 순간
        {
            WhenPlayerInAir();
            canJumpAgain = true;
        }

        if (isGround)
        {
            powerTimer = 0f;
            whenIsNotInAirPosition = currentPosition;

            staping = false;
        }

        isGround = a;
    }
    private void CharacterHangWallCheck()
    {
        if ((leftWall || rightWall) && !isGround)
        {
            canJumpAgain = true;
            isHangWall = true;
            attacking = false;
        }
        else
        {
            isHangWall = false;
        }
    }
    private void UpWallCheck()
    {
        bool a = Physics2D.OverlapCircle(UpWallChecker.position, 0.05f, whatIsGround);

        upWall = a;
    }
    private void LeftWallCheck()
    {
        bool a = Physics2D.OverlapCircle(LeftWallChecker.position, 0.1f, whatIsGround);

        if (a)
        {
            whenIsNotInAirPosition = currentPosition;
        }

        leftWall = a;
    }
    private void RightWallCheck()
    {
        bool a = Physics2D.OverlapCircle(RightWallChecker.position, 0.1f, whatIsGround);

        if (a)
        {
            whenIsNotInAirPosition = currentPosition;
        }

        rightWall = a;
    }
    private void SetStaping()
    {
        staping = false;
    }

    private void LRCheck(float XMove)
    {
        if (playerInput.moveBtnDown)
        {
            powerTimer = 0f;
        }

        if (!(IsHang || isReflect))
        {
            if (XMove < 0f)
            {
                spriteRenderer.flipX = true;
            }
            else if (XMove > 0f)
            {
                spriteRenderer.flipX = false;
            }

            if (!(isJump || staping || attacking) && isGround)
            {
                attacking = false;

                if (XMove != 0f)
                {
                    anim.Play(characterName + "Run");
                }
                else
                {
                    anim.Play(characterName + "Idle");
                }
            }
        }
    }
}
