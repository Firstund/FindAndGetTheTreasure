using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour, IBossSkill
{
    private GameManager gameManager = null;
    private BossStatus bossStatus = null;
    
    [Header("함수 DoSkill이 실행될 때 실행될 애니메이션 트리거의 이름, 없으면 비워둔다.")]
    [SerializeField]
    private string doAnimationTriggerName = "";

    [SerializeField]
    private float damage = 2f;
    [SerializeField]
    private float canAttackDistance = 3f;

    private bool isAttack = false;
    private bool attacked = false;

    private void Awake() 
    {
        gameManager = GameManager.Instance;
        bossStatus = GetComponent<BossStatus>();
    }

    public string GetSkillScriptName()
    {
        return "MeleeAttack";
    }
    public void DoSkill()
    {
        if(bossStatus.DistanceWithPlayer > canAttackDistance) // 스킬 실행 실패조건을 정의
        {
            // 스킬 실행에 실패했을 때 작동될 코드
            isAttack = false;

            bossStatus.DoCurrentSkillFail();

            return;
        }

        bossStatus.Anim.SetTrigger(doAnimationTriggerName);

        bossStatus.ClearFailedBossSkillNumList();
        bossStatus.LRCheckByPlayer();

        isAttack = true;
    }
    private void GetDamage()
    {
        if(isAttack && bossStatus.DistanceWithPlayer <= canAttackDistance)
        {
            Debug.Log("Hit Player");

            gameManager.player.Hurt(damage);
        }
    }
    private void AttackEnd()
    {
        bossStatus.Anim.SetTrigger("Idle");
        
        isAttack = false;

        bossStatus.DoCurrentSkillSuccess();
    }
}
