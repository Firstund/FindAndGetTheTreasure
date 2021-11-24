using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStatus : MonoBehaviour
{
    private Animator anim = null;
    public Animator Anim
    {
        get { return anim; }
    }

    private List<IBossSkill> bossSkills = null;
    private int currentSkillNum = 0;

    private void Awake()
    {
        anim = GetComponent<Animator>();

        bossSkills = GetComponents<IBossSkill>().ToList();
    }
    private void Start()
    {
        RandomSetSKillNum();
        DoCurrentSkill();
    }
    public void DoCurrentSkill()
    {
        DebugBossSkillName(bossSkills[currentSkillNum]);

        bossSkills[currentSkillNum].DoSkill();
    }
    private void RandomSetSKillNum()
    {
        if (bossSkills.Count > 0)
        {
            int num = UnityEngine.Random.Range(0, bossSkills.Count - 1);

            DebugBossSkillName(bossSkills[num]);

            currentSkillNum = num;
        }
        else
        {
            Debug.LogError(gameObject.name + " has no bossSkill.");
        }
    }
    private void SetSKillNum(int num)
    {
        try
        {
            DebugBossSkillName(bossSkills[num]);

            currentSkillNum = num;
        }
        catch (IndexOutOfRangeException)
        {
            Debug.LogError(num + " is not Match for " + gameObject.name);
        }
        catch (Exception)
        {
            Debug.LogError("SetSkillNum Error in " + this.name + " of " + gameObject.name);
        }
    }
    private void DebugBossSkillName(IBossSkill b)
    {
        Debug.Log("Current Skill is '" + b.GetSkillScriptName() + "'.");
    }
}
