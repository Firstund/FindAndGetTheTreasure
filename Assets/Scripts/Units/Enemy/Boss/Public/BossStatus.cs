using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStatus : MonoBehaviour
{
    private GameManager gameManager = null;

    private Animator anim = null;
    public Animator Anim
    {
        get { return anim; }
    }

    private List<IBossSkill> bossSkills = new List<IBossSkill>();
    private List<int> failedBossSkillNums = new List<int>(); // DoSkill함수 실행에 실패한 스킬들의 모임

    private int currentSkillNum = 0;

    public float DistanceWithPlayer
    {
        get { return Vector2.Distance(transform.position, gameManager.player.transform.position); }
    }

    private void Awake()
    {
        gameManager = GameManager.Instance;

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
    public void DoCurrentSkillFail()
    {
        failedBossSkillNums.Add(currentSkillNum);

        RandomSetSKillNum();
    }
    public void ClearFailedBossSkillNumList()
    {
        Debug.Log("bbb"); // 실행순서 문제

        failedBossSkillNums.Clear();
    }
    public void RandomSetSKillNum()
    {
        if (bossSkills.Count > 0)
        {
            int num;

            while (true)
            {
                num = UnityEngine.Random.Range(0, bossSkills.Count);

                for (int i = 0; i < failedBossSkillNums.Count; i++)
                {
                    if (failedBossSkillNums[i] == num)
                    {
                        continue;
                    }
                }

                break;
            }

            DebugBossSkillName(bossSkills[num]);

            currentSkillNum = num;
        }
        else
        {
            Debug.LogError(gameObject.name + " has no bossSkill.");
        }
    }
    public void SetSkillNum(int num)
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
            Debug.LogError("SetSkillNum Error in SetSkillNum of " + gameObject.name);
        }
    }
    private void DebugBossSkillName(IBossSkill b)
    {
        Debug.Log("Current Skill is '" + b.GetSkillScriptName() + "'.");
    }
}
