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
    
    private bool isAttack = false;
    private bool isPursue = false;
    private bool isSearching = false;
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
        if(canAttack && enemyStat.currentStatus == Status.Attack)
        {
            isAttack = true;
            isPursue = false;
            isSearching = false;
        }
        else if(enemyStat.currentStatus == Status.Found)
        {
            isAttack = false;
            isPursue = true;
            isSearching = false;
        }
        else if(enemyStat.currentStatus == Status.Searching)
        {
            isAttack = false;
            isPursue = false;
            isSearching = true;
        }
    }
    private void FixedUpdate()
    {  
        currentPosition = transform.position;
        playerPosition = enemyStat.playerPosition.position;

        Pursue();
        Searching();

        transform.position = currentPosition;
    }
    private void FlipCheck(Vector2 targetPosition)
    {
        spriteRenderer.flipX = enemyStat.searchCharacter.CheckFlip(targetPosition);
    }
    private void Pursue()
    {
        if(isPursue)
        {
            anim.Play("Move");
            currentPosition = Vector2.MoveTowards(currentPosition, playerPosition, enemyStat.pursueSpeed * Time.fixedDeltaTime);
            FlipCheck(playerPosition);
        }
    }
    private void Attack()
    {
        if(isAttack)
        {
            anim.Play("Attack");
            FlipCheck(playerPosition);
        }
    }
    private void Searching()
    {
        float distance;
        if(isSearching)
        {
            if(serachMove)
            {
                anim.Play("Move");
                currentPosition = Vector2.MoveTowards(currentPosition, searchTargetPosition, enemyStat.searchSpeed * Time.fixedDeltaTime);

                distance = Vector2.Distance(currentPosition, searchTargetPosition);

                if(distance <= searchResetDistance)
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

        if(enemyStat.isAirEnemy)
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
