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
    private CharacterStat characterStat = null;
    public CharacterStat CharacterStat
    {
        get { return characterStat; }
    }

    private CharacterTimeWarp characterTimeWarp = null;

    [SerializeField]
    private GameObject pulley = null;
    [Header("여기부터 사운드박스들")]
    [SerializeField]
    private GameObject hurtSoundBox = null;
    [SerializeField]
    private GameObject jumpSoundBox = null;
    [SerializeField]
    private GameObject stapSoundBox = null;

    [Header("여기부터 이펙트들")]
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
    public SkillUseSpValue SkillUseValue
    {
        get { return skillUseValue; }
    }

    [SerializeField]
    private LayerMask whatIsGround;
    public LayerMask WhatIsGround
    {
        get
        {
            return whatIsGround;
        }
    }
    [SerializeField]
    private LayerMask whatIsEnemy;
    public LayerMask WhatIsEnemy
    {
        get
        {
            return whatIsEnemy;
        }
    }
    private LayerMask whatIsDashAttackable;
    public LayerMask WhatIsDashAttackable
    {
        get
        {
            return whatIsDashAttackable;
        }
    }
    [SerializeField]
    private LayerMask whatIsPorjectile;

    [SerializeField]
    private Transform groundChecker;
    public Transform GroundChecker
    {
        get
        {
            return groundChecker;
        }
        set
        {
            groundChecker = value;
        }
    }
    [SerializeField]
    private Transform leftWallChecker;
    public Transform LeftWallChecker
    {
        get
        {
            return leftWallChecker;
        }
        set
        {
            leftWallChecker = value;
        }
    }
    [SerializeField]
    private Transform rightWallChecker;
    public Transform RightWallChecker
    {
        get
        {
            return rightWallChecker;
        }
        set
        {
            rightWallChecker = value;
        }
    }
    [SerializeField]
    private Transform upWallChecker;
    public Transform UpWallChecker
    {
        get
        {
            return upWallChecker;
        }
        set
        {
            upWallChecker = value;
        }
    }

    private bool isGround = false;
    public bool IsGround
    {
        get { return isGround; }
    }
    private bool leftWall = false;
    private bool rightWall = false;
    private bool upWall = false;

    private bool isJump = false;
    private bool isHang = false;
    public bool IsHang
    {
        get
        {
            return isHang;
        }
        set
        {
            isHang = value;
        }
    }
    private bool isDash = false;
    public bool IsDash
    {
        get
        {
            return isDash;
        }
        set
        {
            isDash = value;
        }
    }
    private bool isDead = false;
    public bool IsDead
    {
        get { return isDead; }
    }
    private bool isAttack = false;
    public bool IsAttack 
    {
        get
        {
            return isAttack;
        }
        set
        {
            isAttack = value;
        }
    }
    private bool isHurt = false;
    private bool _canHurt = true;
    public bool canHurt
    {
        get { return _canHurt; }
    }
    private bool isHangWall = false;
    public bool IsHangWall
    {
        get
        {
            return isHangWall;
        }
        set
        {
            isHangWall = value;
        }
    }
    private bool isReflect = false;
    public bool IsReflect
    {
        get { return isReflect; }
        set { isReflect = value; }
    }
    private bool canDash = true;
    public bool CanDash
    {
        get
        {
            return canDash;
        }
        set
        {
            canDash = value;
        }
    }
    private bool dashMoving = false;
    public bool DashMoving
    {
        get
        {
            return dashMoving;
        }
        set
        {
            dashMoving = value;
        }
    }
    private bool staping = false;
    public bool Staping
    {
        get
        {
            return staping;
        }
        set
        {
            staping = value;
        }
    }
    private bool attacking = false;
    public bool Attacking
    {
        get
        {
            return attacking;
        }
        set
        {
            attacking = value;
        }
    }
    private bool dashAttacking = false;
    public bool DashAttacking
    {
        get
        {
            return dashAttacking;
        }
        set
        {
            dashAttacking = value;
        }
    }
    private bool canJumpAgain = false;

    [Header("떨어지기 시작했을 때 이 값만큼 아래로 이동하면, 낙사처리")]
    [SerializeField]
    private float dropValue = 5f;
    private Vector2 whenIsNotInAirPosition = Vector2.zero; // 공중에 떨어져 있지 않을 때의 위치

    public Vector2 currentPosition { get; private set; }

    private float firstGravity = 0f;
    private float firstMass = 0f;

    public event Action WhenPlayerDead;
    public event Action WhenInAirToGround;

    // TODO: OnCollision을 이용하여 벽에 붙었을 때 서서히 내려감

    private void Awake()
    {
        gameManager = GameManager.Instance;
        stageManager = StageManager.Instance;

        rigid = GetComponent<Rigidbody2D>();
        boxCol2D = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        pulleySpriteRenderer = pulley.GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        playerInput = GetComponent<PlayerInput>();
        characterStat = GetComponent<CharacterStat>();
        characterTimeWarp = GetComponent<CharacterTimeWarp>();
        spawnEffect = GetComponent<SpawnEffect>();

        WhenPlayerDead = () =>
        {

        };

        WhenInAirToGround = () =>
        {

        };
    }

    void Start()
    {
        stageManager.SetPlayerRespawnPosition(transform.position);

        if (playerInput == null)
        {
            playerInput = gameObject.AddComponent<PlayerInput>();
        }

        characterName = characterStat.characterName;
        firstGravity = rigid.gravityScale;
        firstMass = rigid.mass;

        whatIsDashAttackable.value = whatIsEnemy + whatIsGround;

        characterStat.hp = characterStat.firstHp;

        currentPosition = transform.position;
        whenIsNotInAirPosition = currentPosition;

        pulley.SetActive(false);
    }
    private void OnEnable()
    {
        stageManager.PlayerRespawn += PlayerRespawn;

        isDead = false;
        attacking = false;

        spriteRenderer.color = new Vector4(1f, 1f, 1f, 1f);
    }

    private void PlayerRespawn()
    {
        whenIsNotInAirPosition = currentPosition;
        characterStat.hp = characterStat.firstHp;
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
        }
        else if (gameManager.stopTime && !characterTimeWarp.isTimeWarp)
        {
            anim.speed = 0f;
        }

        SetAnimByTimeWarp();
        CheckDead();

        if (!upWall)
        {
            IsHang = false;
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
                reflectEffect.transform.position = spriteRenderer.flipX ? rightWallChecker.position : leftWallChecker.position;
            }
            else
            {
                Jump();
                Hang();
                MoveX(XMove);

                InAirCheck();

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
                        spawnEffect.ShowEffect(slideAtSideWall, leftWallChecker.position);
                    }
                    else
                    {
                        spawnEffect.ShowEffect(slideAtSideWall, rightWallChecker.position);
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

            if (IsHang)
            {
                IsHang = false;
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
        if (!IsHang)
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
        bool a = Physics2D.OverlapCircle(groundChecker.position, 0.05f, whatIsGround);

        if (!isGround && a) // 착지하는 순간
        {
            // SetAttacking();

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

            WhenInAirToGround?.Invoke();

            canJumpAgain = false;
        }

        if (isGround && !a)// 공중으로 떨어진 순간
        {
            canJumpAgain = true;
        }

        if (isGround)
        {
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
        bool a = Physics2D.OverlapCircle(upWallChecker.position, 0.05f, whatIsGround);

        if (!a && IsHang)
        {
            IsHang = false;
        }

        upWall = a;
    }
    private void LeftWallCheck()
    {
        bool a = Physics2D.OverlapCircle(leftWallChecker.position, 0.1f, whatIsGround);

        if (a)
        {
            whenIsNotInAirPosition = currentPosition;
        }

        leftWall = a;
    }
    private void RightWallCheck()
    {
        bool a = Physics2D.OverlapCircle(rightWallChecker.position, 0.1f, whatIsGround);

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
