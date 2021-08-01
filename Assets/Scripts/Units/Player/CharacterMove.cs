using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;

public class CharacterMove : MonoBehaviour
{
    private GameManager gameManager = null;
    private StageManager stageManager = null;

    private string characterName;
    private Rigidbody2D rigid = null;
    private BoxCollider2D boxCol2D = null;
    private Animator anim = null;
    public SpriteRenderer spriteRenderer { get; private set; }
    private SpriteRenderer pulleySpriteRenderer = null;

    private PlayerInput playerInput = null;
    private SpawnAfterImage spawnAfterImage = null;
    private CharacterStat characterStat = null;

    [SerializeField]
    private GameObject pulley = null;

    [SerializeField]
    private LayerMask whatIsGround;
    [SerializeField]
    private LayerMask whatIsEnemy;

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

    private bool isJump = false;
    private bool isHang = false;
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
    private bool canDash = true;
    private bool canDashAttack = true;
    private bool dashMoving = false;
    private bool staping = false;
    private bool attacking = false;
    private bool canJumpAgain = false;

    private bool canSpawnAfterImageByDash = true;

    private bool whenOutHangMove = false;
    private bool whenOutHangMoveStarted = false;

    private Vector2 dashPosition = Vector2.zero;

    public Vector2 currentPosition { get; private set; }

    private float firstGravity = 0f;

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
    }

    void Start()
    {
        gameManager = GameManager.Instance;
        stageManager = FindObjectOfType<StageManager>();

        if (playerInput == null)
        {
            playerInput = gameObject.AddComponent<PlayerInput>();
        }

        characterName = characterStat.characterName;
        firstGravity = rigid.gravityScale;
    }
    void Update()
    {
        if (!isDead)
        {
            if (playerInput.isJump && !staping)
            {
                attacking = false;
                if (upWall)
                {
                    if (!isHang)
                    {
                        isHang = true;
                        whenOutHangMoveStarted = false;
                        isJump = false;
                    }
                    else
                    {
                        isHang = false;
                    }
                }
                else
                {
                    isJump = true;
                    isHang = false;
                }
            }

            if (!upWall)
            {
                isHang = false;
            }

            if (playerInput.isDash)
            {
                isDash = true;
            }

            if (playerInput.isAttack && !attacking)
            {
                isAttack = true;
            }

            GroundCheck();
            UpWallCheck();
            LeftWallCheck();
            RightWallCheck();
            CharacterHangWallCheck();
        }

        if (characterStat.hp <= 0f)
        {
            isDead = true;
            isJump = false;
            isDash = false;
            isAttack = false;
            isGround = false;

            Dead();
        }
    }
    void FixedUpdate()
    {
        if (!isDead)
        {
            currentPosition = transform.position;

            float XMove = playerInput.XMove;

            LRCheck(XMove);

            MoveX(XMove);

            Jump();
            Hang();
            InAirCheck();
            Dash(XMove);

            Attack();

            DashMove();
            SpawnAfterImageByDash();

            transform.position = currentPosition;

        }
    }
    private void Hang()
    {
        if (isHang)
        {
            pulleySpriteRenderer.flipX = spriteRenderer.flipX;
            pulley.SetActive(true);

            rigid.gravityScale = -1f;

            anim.Play(characterName + "Hang");

            whenOutHangMove = true;
        }
        else
        {
            rigid.gravityScale = firstGravity;

            pulley.SetActive(false);

            if (whenOutHangMove)
            {

                if (spriteRenderer.flipX)
                {
                    rigid.velocity = new Vector2(-1f * characterStat.speed, rigid.velocity.y);
                }
                else
                {
                    rigid.velocity = new Vector2(1f * characterStat.speed, rigid.velocity.y);
                }

                if (!whenOutHangMoveStarted)
                {
                    Invoke("WhenOutHangMoveSet", 1f);
                    whenOutHangMoveStarted = true;
                }
            }
        }
    }
    private void WhenOutHangMoveSet()
    {
        whenOutHangMove = false;
    }
    public void _Hurt()
    {
        if (!isHurt && canHurt)
        {
            isHurt = true;
            _canHurt = false;
            stageManager.ShakeCamera(1f, 0.1f);

            StartCoroutine(Hurt());

            Invoke("isHurtSet", 1f);

        }
    }
    private IEnumerator Hurt()
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
        gameObject.SetActive(false);
    }
    private void Dash(float XMove)
    {
        if (XMove != 0 && isDash && !isHang && !staping && !dashMoving && canDash)
        {
            float _dashRange = dashRange;
            dashPosition = GroundChecker.position;

            dashPosition.y += 0.2f;

            if (attacking)
            {
                gameManager.SetSlowTime(0.1f);
                _dashRange = DashAttack(_dashRange);
            }
            else
            {
                canDashAttack = false;
                attacking = false;
            }

            if (spriteRenderer.flipX)
            {
                _dashRange = -_dashRange;
            }

            Vector2 endPosition = currentPosition;

            endPosition.x = currentPosition.x + _dashRange;

            dashPosition = positionCantCrossWall(dashPosition, endPosition);

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

    private Vector2 positionCantCrossWall(Vector2 originPosition, Vector2 endPosition)
    {
        bool a = false;
        bool b = false;
        do
        {
            a = Physics2D.OverlapCircle(originPosition, 0.1f, whatIsGround);
            if (!a)
            {
                if (spriteRenderer.flipX)
                {
                    originPosition.x -= 0.1f;
                }
                else
                {
                    originPosition.x += 0.1f;
                }
            }

            if (spriteRenderer.flipX)
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
        } while (!a && !b);

        return originPosition;
    }

    private float DashAttack(float _dashRange)
    {
        if (canDashAttack)
        {
            anim.Play(characterName + "DashAttack");

            _dashRange = dashRange * (2f / 3f);

            Vector2 endPosition = currentPosition;

            dashPosition = GroundChecker.position;

            dashPosition.y += 0.2f;


            endPosition.x = currentPosition.x + _dashRange;

            dashPosition = positionCantCrossWall(dashPosition, endPosition);

            GetDashAttackDamage();
        }

        return _dashRange;
    }

    private void DashRe()
    {
        canDash = true;
        canDashAttack = true;
    }
    private void SpawnAfterImageByDash()
    {
        if (dashMoving && canSpawnAfterImageByDash)
        {
            float spawnAfterImageDelay = Random.Range(spawnAfterImage.spawnAfterImageDelayMinimum, spawnAfterImage.spawnAfterImageDelayMaximum);
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
        if (!attacking && !dashMoving && !isHangWall && !staping && isAttack) //isGround에 따라서 GroundAttack과 InAirAttack을 나눌것, dashing == true라면 dashAttack을 할것
        {
            if (isGround)
            {
                attacking = true;

                anim.Play(characterName + "Attack");

                isAttack = false;
            }
            else if (!isGround)
            {
                attacking = true;
                anim.Play(characterName + "InAirAttack");

                isAttack = false;
                isHang = false;
            }

            GetDamage();
        }
    }
    private void GetDamage()
    {
        Collider2D[] a = Physics2D.OverlapCircleAll(currentPosition, characterStat.attackRange, whatIsEnemy);

        foreach (var item in a)
        {
            EnemyStat enemyStat = item.GetComponent<EnemyStat>();
            EnemyMove enemyMove = item.GetComponent<EnemyMove>();

            float enemyHp = enemyStat.hp;
            float enemyDp = enemyStat.dp;

            float totalDamage = characterStat.ap - enemyDp;

            if (totalDamage <= 0f)
            {
                totalDamage = 0.5f;
            }

            enemyHp -= totalDamage;

            enemyStat.hp = enemyHp;

            enemyMove._Hurt();
        }
    }
    private void GetDashAttackDamage()
    {
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
                targetPositions[i] = endPosition;
            }
        }
        else
        {
            for (int i = 0; i < 5; i++)
            {
                Vector2 endPosition = new Vector2(targetPositions[i].x + attackRange, targetPositions[i].y);
                targetPositions[i] = endPosition;
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
            RaycastHit2D[] hit = Physics2D.RaycastAll(rays[i].origin, rays[i].direction, attackRange, whatIsEnemy);

            Debug.DrawRay(rays[i].origin, rays[i].direction, Color.red, 10f);

            foreach (var item in hit)
            {
                hits.Add(item);
            }
        }

        hits = hits.Distinct().ToList();

        foreach (var item in hits)
        {
            EnemyStat enemyStat = item.transform.GetComponent<EnemyStat>();
            EnemyMove enemyMove = item.transform.GetComponent<EnemyMove>();

            if (enemyMove != null && enemyStat != null)
            {
                float enemyHp = enemyStat.hp;
                float enemyDp = enemyStat.dp;

                float totalDamage = characterStat.ap - enemyDp;

                if (totalDamage <= 0f)
                {
                    totalDamage = 0.5f;
                }

                enemyHp -= totalDamage;

                enemyStat.hp = enemyHp;

                enemyMove._Hurt();
            }
        }
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

            anim.Play(characterName + "Jump");
        }
    }
    public void Jumping()
    {
        rigid.velocity = new Vector2(rigid.velocity.x, characterStat.jumpSpeed);

        isJump = false;
    }
    private void InAirCheck()
    {
        if (!(isJump || isHangWall || isGround || isHang || staping || attacking))
        {
            anim.Play(characterName + "InAir");
        }
    }

    private void MoveX(float XMove)
    {
        if (!isHang)
        {
            // if ((leftWall || rightWall) && !isGround)
            // {
            //     if (leftWall && spriteRenderer.flipX && !isJump)
            //     {
            //         rigid.velocity = new Vector2(0f, -1f);
            //     }
            //     else if (rightWall && !spriteRenderer.flipX && !isJump)
            //     {
            //         rigid.velocity = new Vector2(0f, -1f);
            //     }
            // }
            // else
            {
                rigid.velocity = new Vector2(XMove * characterStat.speed, rigid.velocity.y);
            }
        }
        else
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
            anim.Play(characterName + "Stap");
            canJumpAgain = false;
        }

        if (isGround && !a)// 공중으로 떨어진 순간
        {
            canJumpAgain = true;
        }

        if (isGround)
        {
            WhenOutHangMoveSet();
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

        leftWall = a;
    }
    private void RightWallCheck()
    {
        bool a = Physics2D.OverlapCircle(RightWallChecker.position, 0.1f, whatIsGround);

        rightWall = a;
    }
    private void SetStaping()
    {
        staping = false;
    }

    private void LRCheck(float XMove)
    {
        if (!isHang)
        {
            if (XMove < 0f)
            {
                spriteRenderer.flipX = true;
            }
            else if (XMove > 0f)
            {
                spriteRenderer.flipX = false;
            }

            if (!isJump && !staping && !attacking)
            {
                attacking = false;

                if (isHangWall)
                {
                    anim.Play(characterName + "HangWall");
                }
                else if (isGround)
                {
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
}
