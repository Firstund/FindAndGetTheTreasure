using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMoveToPlayer : BossSkillBase
{
    private Rigidbody2D rigid = null;
    private SpriteRenderer spriteRenderer = null;

    private Vector2 targetPos = Vector2.zero;
    private Vector2 moveDir
    {
        get { return (targetPos - (Vector2)transform.position).normalized; }
    }

    [SerializeField]
    private float moveSpeed = 1f;

    [Header("플레이어와의 거리가 이 값과 같거나 보다 작으면 이동 완료로 간주하고 멈춘다.")]
    [SerializeField]
    private float moveStopDistance = 2f;

    [Header("움직이기 시작하고 나서부터 지난 시간(초)가 이 값보다 크면 DoCurrentSkillSuccess처리")]
    [SerializeField]
    private float moveTime = 2f;
    private float moveTimer = 0f;

    private float distance
    {
        get { return Vector2.Distance(transform.position, targetPos); }
    }

    private bool isMove = false;

    public void Awake()
    {
        DoAwake();
        gameManager = GameManager.Instance;
        stageManager = StageManager.Instance;

        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        bossStatus = GetComponent<BossStatus>();
    }

    void FixedUpdate()
    {
        if (doThisSkill)
        {
            if(moveTimer < moveTime)
            {
                moveTimer += Time.fixedDeltaTime;
            }
            else
            {
                MoveDone();
            }
            
            MoveToPlayer();
        }
    }
    public override string GetSkillScriptName()
    {
        return "BossMoveToPlayer";
    }
    public override void DoSkill()
    {
        if (doThisSkill)
        {
            base.DoSkill();
            targetPos = gameManager.player.transform.position;

            bossStatus.ClearFailedBossSkillNumList();
            bossStatus.LRCheckByPlayer();

            moveTimer = 0f;
            isMove = true;
        }
    }
    private void MoveToPlayer()
    {
        float speed = moveSpeed;

        if (isMove)
        {
            if (gameManager.stopTime)
            {
                speed = 0f;
            }

            if (!bossStatus.IsAirUnit)
            {
                targetPos.y = transform.position.y;
            }

            if (bossStatus.IsAirUnit)
            {
                rigid.velocity = moveDir * speed;
            }
            else
            {
                rigid.velocity = new Vector2((moveDir * speed).x, rigid.velocity.y);
            }

            if (distance <= moveStopDistance)
            {
                MoveDone();
            }
        }
    }

    private void MoveDone()
    {
        isMove = false;

        rigid.velocity = Vector2.zero;

        doThisSkill = false;

        bossStatus.DoCurrentSkillSuccess();

        bossStatus.Anim.SetTrigger("Idle");
    }
}
