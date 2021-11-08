using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : EnemyStatus
{
    private GameManager gameManager = null;
    private StageManager stageManager = null;
    private Animator anim = null;
    private SpriteRenderer spriteRenderer = null;
    private EnemyStat enemyStat = null;
    private Rigidbody2D rigid = null;

    [SerializeField]
    private float searchResetDelay = 1f;
    [SerializeField]
    private float searchResetDistance = 0.5f;
    [SerializeField]
    private float searchRangeX = 1f;
    [Header("이 유닛이 공중유닛일 경우에만 적용되는 값")]
    [SerializeField]
    private float searchRangeY = 1f;

    private float searchResetTime = 5f;
    private float searchResetTimer = 0f;

    [SerializeField]
    private GameObject projectile = null;

    [SerializeField]
    private LayerMask whatIsPlayer;
    [SerializeField]
    private LayerMask whatIsGround;

    private bool isAttack = false;
    private bool isShoot = false;
    private bool isSearching = false;
    private bool isPursue = false;
    private bool isDead = false;
    private bool isHurt = false;
    private bool isInWall = false;
    private bool searchMove = true;
    private bool canAttack = true;

    private bool stopMyself = false;
    private bool firstSearchPositionSet = false;

    [SerializeField]
    private float pursueTime = 3f;
    private float pursueTimer = 0f;
    private float stopMyselfTime = 0.5f;
    private float stopMyselfTimer = 0f;


    private Vector2 currentPosition = Vector2.zero;
    private Vector2 playerPosition = Vector2.zero;
    private Vector2 searchTargetPosition = Vector2.zero;

    private Color color = new Color(1f, 0f, 1f, 0.5f);
    private Color color_origin = new Color(1f, 1f, 1f, 1f);

    private RaycastHit2D wallHit = new RaycastHit2D();

    private float firstGravity = 0f;
    private float firstMass = 0f;

    private void Awake()
    {
        gameManager = GameManager.Instance;

        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();

        enemyStat = GetComponent<EnemyStat>();
    }
    void Start()
    {
        stageManager = StageManager.Instance;
        currentPosition = transform.position;

        if (enemyStat.isAirEnemy)
        {
            firstSearchPositionSet = true;
            SearchPositionSet();
        }

        searchResetTimer = searchResetTime;
        firstGravity = rigid.gravityScale;
        firstMass = rigid.mass;
    }

    void Update()
    {
        if (!enemyStat.IsOutCamera) // 카메라 밖으로 나간 처리 후 일정 시간이 지나야 멈추도록 바꾸자
        {
            if (!(isDead || gameManager.stopTime))
            {
                if (anim.speed == 0f)
                {
                    anim.speed = 1f;
                }

                if (!stopMyself)
                {
                    CheckStatus();

                    SetIsPursue();
                    SetSearchResetTimer();
                }

                CheckDead();
            }
            else if (gameManager.stopTime)
            {
                anim.speed = 0f;
            }

            if (stopMyselfTimer > 0f)
            {
                stopMyselfTimer -= Time.deltaTime;

                spriteRenderer.color = color;
            }
            else
            {
                stopMyself = false;
                spriteRenderer.color = color_origin;
            }

            if (isDead)
            {
                Dead();
            }
        }
    }

    private void CheckDead()
    {
        if (enemyStat.hp <= 0f)
        {
            isShoot = false;
            isAttack = false;
            isPursue = false;
            isSearching = false;
            isDead = true;
        }
    }

    private void CheckStatus()
    {
        if (enemyStat.currentStatus == Status.Shoot)
        {
            isShoot = true;
            isAttack = false;
            pursueTimer = 0f;
            isSearching = false;
        }
        else if (enemyStat.currentStatus == Status.Attack)
        {
            isShoot = false;
            isAttack = true;
            pursueTimer = 0f;
            isSearching = false;
        }
        else if (enemyStat.currentStatus == Status.Found)
        {
            isShoot = false;
            isAttack = false;
            pursueTimer = pursueTime;
            isSearching = false;
        }
        else if (enemyStat.currentStatus == Status.Searching)
        {
            isShoot = false;
            isAttack = false;
            isSearching = true;
        }
    }

    private void SetSearchResetTimer()
    {
        if (searchResetTimer > 0f)
        {
            searchResetTimer -= Time.deltaTime;
        }
        else
        {
            SearchPositionSet();
        }
    }

    private void SetIsPursue()
    {
        if (pursueTimer > 0f)
        {
            pursueTimer -= Time.deltaTime;

            isPursue = true;
            isSearching = false;

        }
        else
        {
            isPursue = false;
        }
    }

    private void FixedUpdate()
    {
        if (!enemyStat.IsOutCamera)
        {
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

            if (gameManager.SlowTimeSomeObjects && gameManager.CurrentSlowTimePerSlowTime == 0)
            {
                DoFixedUpdate();

                anim.speed = 1f / gameManager.SlowTimeNum;
                rigid.gravityScale = firstGravity / gameManager.SlowTimeNum;
                rigid.mass = firstMass / gameManager.SlowTimeNum;
            }
            else if (!gameManager.SlowTimeSomeObjects)
            {
                DoFixedUpdate();

                anim.speed = 1f;
                rigid.gravityScale = firstGravity;
                rigid.mass = firstMass;
            }
        }
    }

    private void DoFixedUpdate()
    {
        if (!(isDead || gameManager.stopTime || stopMyself))
        {
            currentPosition = transform.position;
            playerPosition = enemyStat.playerPosition.position;

            Pursue();
            Attack();

            if (enemyStat.isShootProjectile)
            {
                Shoot();
            }

            Searching();

            transform.position = currentPosition;
        }
    }
    private void OnEnable()
    {
        spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
        enemyStat.hp = enemyStat.firstHp;
        isDead = false;
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (enemyStat.isAirEnemy)
        {
            if (other.gameObject.tag == "GROUND")
            {
                isInWall = true;
                isSearching = false;

                Ray2D ray = new Ray2D();

                ray.origin = transform.position;
                ray.direction = other.transform.position;

                wallHit = Physics2D.Raycast(ray.origin, ray.direction, 2f, whatIsGround);
            }
        }
        else if (!firstSearchPositionSet && other.gameObject.tag == "GROUND")
        {
            firstSearchPositionSet = true;
            SearchPositionSet();
        }
    }
    private void OnCollisionExit2D(Collision2D other)
    {
        isInWall = false;
    }

    private void MoveEnemy(Vector2 targetPos, float speed)
    {
        if (gameManager.SlowTimeSomeObjects)
        {
            currentPosition = Vector2.MoveTowards(currentPosition, targetPos, speed / gameManager.SlowTimeNum * Time.fixedDeltaTime);
        }
        else
        {
            currentPosition = Vector2.MoveTowards(currentPosition, targetPos, speed * Time.fixedDeltaTime);
        }
    }

    private void FlipCheck(Vector2 targetPosition)
    {
        spriteRenderer.flipX = enemyStat.searchCharacter.CheckFlip(targetPosition);
    }
    private void Pursue()
    {
        if (isPursue)
        {
            anim.Play("Move");

            MoveEnemy(playerPosition, enemyStat.pursueSpeed);

            FlipCheck(playerPosition);
        }
    }
    private void Searching()
    {
        float distance;
        if (isSearching && firstSearchPositionSet)
        {
            if (searchMove && !isInWall)
            {
                anim.Play("Move");

                MoveEnemy(searchTargetPosition, enemyStat.searchSpeed);

                if (enemyStat.isAirEnemy)
                {
                    distance = Vector2.Distance(currentPosition, searchTargetPosition);
                }
                else
                {
                    Vector2 _currentPosition = currentPosition;
                    _currentPosition.y = searchTargetPosition.y;

                    distance = Vector2.Distance(_currentPosition, searchTargetPosition);
                }

                if (distance <= searchResetDistance)
                {
                    searchMove = false;
                    SearchPositionSet();
                    Invoke("SearMoveReset", searchResetDelay);
                }
            }
            else if (isInWall)
            {
                anim.Play("Move");

                Vector2 targetPos = transform.position;

                if (transform.position.x - wallHit.point.x >= 0f)
                {
                    targetPos.x += 1f;
                }
                else
                {
                    targetPos.x -= 1f;
                }

                if (transform.position.y - wallHit.point.y >= 0f)
                {
                    targetPos.y += 1f;
                }
                else
                {
                    targetPos.y -= 1f;
                }

                MoveEnemy(targetPos, enemyStat.searchSpeed);

                FlipCheck(targetPos);
            }
            else
            {
                anim.Play("Idle");
            }

            FlipCheck(searchTargetPosition);
        }
    }
    private void Attack()
    {
        if (canAttack && isAttack)
        {
            anim.Play("Attack");

            FlipCheck(playerPosition);
        }
        else
        {
            isAttack = false;
        }
    }
    private void Shoot()
    {
        if (canAttack && isShoot)
        {
            anim.Play("Shoot");

            FlipCheck(playerPosition);
        }
        else
        {
            isShoot = false;
        }
    }
    private void Dead()
    {
        gameManager.SetSlowTime(0.3f);
        StartCoroutine(stageManager.SetCameraSize(6.8f, 0.2f));
        anim.Play("Dead");
    }
    private void Destroye()
    {
        stageManager.DespawnEnemy(gameObject);
    }
    private void AttackRe()
    {
        canAttack = true;
    }
    private void ShootProjectile()
    {
        if (canAttack)
        {
            canAttack = false;

            stageManager.ShootEnemyProjectile(projectile, enemyStat, spriteRenderer.flipX, currentPosition, Quaternion.identity, enemyStat.shootRange);

            Invoke("AttackRe", enemyStat.attackDelay);
        }
    }
    private void GetDamage()
    {
        if (canAttack)
        {
            canAttack = false;

            bool a = Physics2D.OverlapCircle(currentPosition, enemyStat.attackRange, whatIsPlayer);

            if (a)
            {
                Collider2D player_Col = Physics2D.OverlapCircle(currentPosition, enemyStat.attackRange, whatIsPlayer);
                CharacterMove characterMove = player_Col.gameObject.GetComponent<CharacterMove>();

                if (characterMove.canHurt)
                {
                    characterMove.Hurt(enemyStat.ap);
                }
            }

            Invoke("AttackRe", enemyStat.attackDelay);
        }
    }
    public void Hurt(float damage)
    {
        float enemyHp = enemyStat.hp;
        float enemyDp = enemyStat.dp;

        float totalDamage = damage - enemyDp;

        if (totalDamage <= 0f)
        {
            totalDamage = 0.5f;
        }

        enemyHp -= totalDamage;

        enemyStat.hp = enemyHp;

        stopMyself = true;
        stopMyselfTimer = stopMyselfTime;

        if (!isHurt)
        {
            isHurt = true;

            stageManager.ShakeCamera(1.5f, 0.1f);
            gameManager.SetSlowTime(0.01f);
            Invoke("isHurtSet", 1f);
        }
    }
    private void isHurtSet()
    {
        isHurt = false;
    }
    private void SearchPositionSet()
    {
        bool hitGround;

        int loopNum = 0;

        do
        {
            searchResetTimer = searchResetTime;

            Vector2 endPosition = currentPosition;

            if (loopNum >= 10)
            {
                break;
            }

            float _searchX = Random.Range(-searchRangeX, searchRangeX);

            endPosition.x += _searchX;

            if (enemyStat.isAirEnemy)
            {
                float _searchY = Random.Range(-searchRangeY, searchRangeY);

                endPosition.y += _searchY;
            }

            if (enemyStat.isAirEnemy)
            {
                hitGround = true;
            }
            else
            {
                hitGround = Physics2D.Raycast(endPosition, endPosition - Vector2.down, transform.localScale.y + 0.5f, whatIsGround);
            }

            searchTargetPosition = StageManager.Instance.PositionCantCrossWall(currentPosition, endPosition, (currentPosition.x > endPosition.x), whatIsGround);

            loopNum++;

        } while (!hitGround);
    }
    private void SearMoveReset()
    {
        searchMove = true;
    }
}
