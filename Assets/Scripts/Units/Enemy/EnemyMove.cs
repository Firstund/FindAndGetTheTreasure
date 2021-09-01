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
    private bool searchMove = true;
    private bool canAttack = true;

    [SerializeField]
    private float pursueTime = 3f;
    private float pursueTimer = 0f;

    private Vector2 currentPosition = Vector2.zero;
    private Vector2 playerPosition = Vector2.zero;
    private Vector2 searchTargetPosition = Vector2.zero;

    void Start()
    {
        gameManager = GameManager.Instance;
        stageManager = FindObjectOfType<StageManager>();

        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        enemyStat = GetComponent<EnemyStat>();

        currentPosition = transform.position;
        SearchPositionSet();

        searchResetTimer = searchResetTime;
    }

    void Update()
    {
        if (!(isDead || gameManager.stopTime))
        {
            CheckStatus();

            SetIsPursue();
            SetSearchResetTimer();

            CheckDead();
        }
        else if (gameManager.stopTime)
        {
            anim.Play("Idle");
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

            Dead();
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
        if (!(isDead || gameManager.stopTime))
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
    public void SpawnSet()
    {
        spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
        enemyStat.hp = enemyStat.firstHp;
        isDead = false;
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
            currentPosition = Vector2.MoveTowards(currentPosition, playerPosition, enemyStat.pursueSpeed * Time.fixedDeltaTime);
            FlipCheck(playerPosition);
        }
    }
    private void Attack()
    {
        if (canAttack && isAttack)
        {
            canAttack = false;
            anim.Play("Attack");
            FlipCheck(playerPosition);
            Invoke("AttackRe", enemyStat.attackDelay);
        }
    }
    private void Shoot()
    {
        if (canAttack && isShoot)
        {
            canAttack = false;
            anim.Play("Shoot");
            FlipCheck(playerPosition);
            Invoke("AttackRe", enemyStat.attackDelay);
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
        stageManager.ShootProjectile(projectile, enemyStat, spriteRenderer.flipX, currentPosition, enemyStat.shootRange);
    }
    private void GetDamage()
    {
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

        if (!isHurt)
        {
            isHurt = true;

            StartCoroutine(hurt());
            stageManager.ShakeCamera(1.5f, 0.1f);
            gameManager.SetSlowTime(0.01f);
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
    }
    private void Searching()
    {
        float distance;
        if (isSearching)
        {
            if (searchMove)
            {
                anim.Play("Move");
                currentPosition = Vector2.MoveTowards(currentPosition, searchTargetPosition, enemyStat.searchSpeed * Time.fixedDeltaTime);

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
            else
            {
                anim.Play("Idle");
            }

            FlipCheck(searchTargetPosition);
        }
    }
    private void SearchPositionSet()
    {
        bool hitGround;

        do
        {
            searchResetTimer = searchResetTime;

            Vector2 endPosition = currentPosition;

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
                hitGround = Physics2D.Raycast(endPosition, Vector2.down, transform.localScale.y + 0.5f, whatIsGround);
            }

            searchTargetPosition = stageManager.PositionCantCrossWall(currentPosition, endPosition, (currentPosition.x > endPosition.x), whatIsGround);
        } while (!hitGround);
    }
    private void SearMoveReset()
    {
        searchMove = true;
    }
}
