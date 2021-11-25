using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMoveToPlayer : MonoBehaviour, IBossSkill
{
    private GameManager gameManager = null;

    private Rigidbody2D rigid = null;

    private BossStatus bossStatus = null;

    [Header("함수 DoSkill이 실행될 때 실행될 애니메이션 트리거의 이름, 없으면 비워둔다.")]
    [SerializeField]
    private string doAnimationTriggerName = "";

    private Vector2 moveDir
    {
        get { return (gameManager.player.transform.position - transform.position).normalized; }
    }

    [SerializeField]
    private float moveSpeed = 1f;

    private float distance
    {
        get { return Vector2.Distance(transform.position, gameManager.player.transform.position); }
    }

    private bool isMove = false;

    private void Awake()
    {
        gameManager = GameManager.Instance;

        rigid = GetComponent<Rigidbody2D>();

        bossStatus = GetComponent<BossStatus>();
    }

    void FixedUpdate()
    {
        MoveToPlayer();
    }
    public string GetSkillScriptName()
    {
        return "BossMoveToPlayer";
    }
    public void DoSkill()
    {
        bossStatus.Anim.SetTrigger(doAnimationTriggerName);

        bossStatus.ClearFailedBossSkillNumList();

        isMove = true;
    }
    private void MoveToPlayer()
    {
        if (isMove && !gameManager.stopTime)
        {
            rigid.velocity = moveDir * moveSpeed;

            if (distance <= 2f)
            {
                bossStatus.Anim.SetTrigger("Idle");

                isMove = false;
            }
        }
    }
    public void SetFalseIsMove()
    {
        isMove = false;
    }
}
