using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour, IBossSkill
{
    private BossStatus bossStatus = null;
    
    [Header("함수 DoSkill이 실행될 때 실행될 애니메이션 트리거의 이름, 없으면 비워둔다.")]
    [SerializeField]
    private string doAnimationTriggerName = "";

    [SerializeField]
    private float damage = 2f;
    [SerializeField]
    private float canAttackDistance = 3f;

    private bool isAttack = false;
    private void Awake() 
    {
        bossStatus = GetComponent<BossStatus>();
    }
    private void Update() 
    {
        if(isAttack)
        {
            Debug.Log("IsAttack!");
        }    
    }

    public string GetSkillScriptName()
    {
        return "MeleeAttack";
    }
    public void DoSkill()
    {
        if(bossStatus.DistanceWithPlayer > canAttackDistance)
        {
            Debug.Log("aaa");
            isAttack = false;

            bossStatus.DoCurrentSkillFail();

            return;
        }

        bossStatus.Anim.SetTrigger(doAnimationTriggerName);

        bossStatus.ClearFailedBossSkillNumList();

        isAttack = true;
    }
    private void AttackEnd()
    {
        isAttack = false;
    }
}
