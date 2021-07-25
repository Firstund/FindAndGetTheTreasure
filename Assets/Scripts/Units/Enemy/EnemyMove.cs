using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : EnemyStatus
{
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

    [SerializeField]
    private LayerMask WhatIsPlayer;

    private bool isAttack = false;
    private bool isPursue = false;
    private bool isSearching = false;
    private bool isDead = false;
    private bool isHurt = false;
    private bool serachMove = true;
    private bool canAttack = true;

    private Vector2 currentPosition = Vector2.zero;
    private Vector2 playerPosition = Vector2.zero;
    private Vector2 searchTargetPosition = Vector2.zero;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        enemyStat = GetComponent<EnemyStat>();
    }

    void Update()
    {
        if (!isDead)
        {
            if (enemyStat.currentStatus == Status.Attack)
            {
                isAttack = true;
                isPursue = false;
                isSearching = false;
            }
            else if (enemyStat.currentStatus == Status.Found)
            {
                isAttack = false;
                isPursue = true;
                isSearching = false;
            }
            else if (enemyStat.currentStatus == Status.Searching)
            {
                isAttack = false;
                isPursue = false;
                isSearching = true;
            }

            if (enemyStat.hp <= 0f)
            {
                isAttack = false;
                isPursue = false;
                isSearching = false;
                isDead = true;

                Dead();
            }
        }
    }
    private void FixedUpdate()
    {
        currentPosition = transform.position;
        playerPosition = enemyStat.playerPosition.position;

        Pursue();
        Attack();
        Searching();

        transform.position = currentPosition;
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
    private void Dead()
    {
        anim.Play("Dead");
    }
    private void Destroye()
    {
        gameObject.SetActive(false);
    }
    private void AttackRe()
    {
        canAttack = true;
    }
    private void GetDamage()
    {
        bool a = Physics2D.OverlapCircle(currentPosition, enemyStat.attackRange, WhatIsPlayer);

        if (a)
        {
            Collider2D player_Col = Physics2D.OverlapCircle(currentPosition, enemyStat.attackRange, WhatIsPlayer);
            CharacterStat _player = player_Col.gameObject.GetComponent<CharacterStat>();
            CharacterMove _playerMove = player_Col.gameObject.GetComponent<CharacterMove>();

            float p_hp = _player.hp;
            float p_dp = _player.dp;

            float totalDamage;

            totalDamage = enemyStat.ap - p_dp;

            if (totalDamage <= 0f)
            {
                totalDamage = 0.5f;
            }

            p_hp -= totalDamage;
            _player.hp = p_hp;

            _playerMove._Hurt();
        }
    }
    public void _Hurt()
    {
        if (!isHurt)
        {
            isHurt = true;

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
    }
    private void Searching()
    {
        float distance;
        if (isSearching)
        {
            if (serachMove)
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
                    serachMove = false;
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
        searchTargetPosition = currentPosition;
        float _searchX = Random.Range(-searchRangeX, searchRangeX);

        searchTargetPosition.x += _searchX;

        if (enemyStat.isAirEnemy)
        {
            float _searchY = Random.Range(-searchRangeY, searchRangeY);

            searchTargetPosition.y += _searchY;
        }
    }
    private void SearMoveReset()
    {
        serachMove = true;
    }
}
