using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BossSkillBase : MonoBehaviour, IBossSkill
{
    protected GameManager gameManager = null;
    protected StageManager stageManager = null;

    protected bool doThisSkill = false;
    public bool DoThisSkill
    {
        get { return doThisSkill; }
        set { doThisSkill = value; }
    }

    protected BossStatus bossStatus = null;
    [Header("함수 DoSkill이 실행될 때 실행될 애니메이션 트리거의 이름, 없으면 비워둔다.")]
    [SerializeField]
    protected string doAnimationTriggerName = "";

    protected void DoAwake()
    {
        gameManager = GameManager.Instance;
        stageManager = StageManager.Instance;

        bossStatus = GetComponent<BossStatus>();
    }

    public virtual void DoSkill()
    {
        if (doThisSkill)
        {
            bossStatus.Anim.SetTrigger(doAnimationTriggerName);
        }
    }
    public virtual string GetSkillScriptName()
    {
        return "noname";
    }
}
