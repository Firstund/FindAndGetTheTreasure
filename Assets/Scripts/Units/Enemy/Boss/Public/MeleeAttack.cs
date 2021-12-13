using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : BossSkillBase
{
    [SerializeField]
    private float damage = 2f;
    [SerializeField]
    private float canAttackDistance = 3f;

    private bool isAttack = false;
    private bool attacked = false;

    private void Awake()
    {
        DoAwake();
    }

    public override string GetSkillScriptName()
    {
        return "MeleeAttack";
    }
    public override void DoSkill()
    {
        if (doThisSkill)
        {
            if (bossStatus.DistanceWithPlayer > canAttackDistance) // 스킬 실행 실패조건을 정의
            {
                // 스킬 실행에 실패했을 때 작동될 코드
                isAttack = false;
                doThisSkill = false;

                bossStatus.DoCurrentSkillFail();

                return;
            }

            base.DoSkill();
            PlaySound();

            bossStatus.ClearFailedBossSkillNumList();
            bossStatus.LRCheckByPlayer();

            isAttack = true;
        }
    }
    private void GetDamage()
    {
        if (isAttack && bossStatus.DistanceWithPlayer <= canAttackDistance)
        {
            Debug.Log("Hit Player");

            gameManager.player.Hurt(damage);
        }
    }
    private void AttackEnd()
    {
        isAttack = false;

        doThisSkill = false;

        bossStatus.DoCurrentSkillSuccess();
    }
}
