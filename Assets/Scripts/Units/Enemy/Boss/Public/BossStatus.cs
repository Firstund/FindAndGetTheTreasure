using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStatus : MonoBehaviour
{
    private GameManager gameManager = null;

    private SpriteRenderer spriteRenderer = null;

    private Animator anim = null;
    public Animator Anim
    {
        get { return anim; }
    }

    private List<IBossSkill> bossSkills = new List<IBossSkill>();
    private List<int> failedBossSkillNums = new List<int>(); // DoSkill함수 실행에 실패한 스킬들의 모임

    [Header("이 보스의 스프라이트 파일들이 왼쪽을 바라보고 있다면 이 값을 true로 해준다.")]
    [SerializeField]
    private bool isLookLeftAtFirst = false;
    public bool IsLookLeftAtFirst
    {
        get { return isLookLeftAtFirst; }
    }

    [SerializeField]
    private bool isAirUnit = false;
    public bool IsAirUnit
    {
        get { return isAirUnit; }
    }

    private int currentSkillNum = 0;

    [SerializeField]
    private float doSkillCycle = 2f;
    public float DistanceWithPlayer
    {
        get { return Vector2.Distance(transform.position, gameManager.player.transform.position); }
    }

    private void Awake()
    {
        gameManager = GameManager.Instance;

        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        bossSkills = GetComponents<IBossSkill>().ToList();
    }
    private void Start()
    {
        RandomSetSKillNum();
    }
    public void LRCheckByPlayer()
    {
        if (gameManager.player.transform.position.x >= transform.position.x) // 플레이어가 오른쪽에 있을 때
        {
            spriteRenderer.flipX = isLookLeftAtFirst;
        }
        else // 플레이어가 왼쪽에 있을 때
        {
            spriteRenderer.flipX = !isLookLeftAtFirst;
        }
    }
    public void LRCheckByPosition(Vector2 pos)
    {
        if (pos.x >= transform.position.x) // 해당 Vector2가 오른쪽에 있을 때
        {
            spriteRenderer.flipX = isLookLeftAtFirst;
        }
        else // 해당 Vector2가 왼쪽에 있을 때
        {
            spriteRenderer.flipX = !isLookLeftAtFirst;
        }
    }
    public void DoCurrentSkill()
    {
        Debug.Log("Do " + bossSkills[currentSkillNum].GetSkillScriptName() + ".");

        bossSkills[currentSkillNum].DoSkill();
    }
    public void DoCurrentSkillFail() // 현재 스킬 실행에 실패했을 때 실행.
    {
        Debug.Log("Do " + bossSkills[currentSkillNum].GetSkillScriptName() + " failed.");

        failedBossSkillNums.Add(currentSkillNum);

        RandomSetSKillNum();
    }
    public void DoCurrentSkillSuccess() // 현재 스킬이 성공적으로 실행되었을 때 실행.
    {
        Debug.Log("Do " + bossSkills[currentSkillNum].GetSkillScriptName() + " successed.");

        RandomSetSKillNum();

        Invoke("DoCurrentSkill", doSkillCycle);
    }
    public void ClearFailedBossSkillNumList()
    {
        Debug.Log("Clear List");
        failedBossSkillNums.Clear();
    }
    public void RandomSetSKillNum()
    {
        if (bossSkills.Count > 0)
        {
            int num;

            while (true)
            {
                bool numSetAgain = false;

                num = UnityEngine.Random.Range(0, bossSkills.Count);

                for (int i = 0; i < failedBossSkillNums.Count; i++)
                {
                    if (failedBossSkillNums[i] == num)
                    {
                        numSetAgain = true;
                    }
                }

                if (numSetAgain)
                {
                    continue;
                }

                break;
            }

            currentSkillNum = num;

            DebugBossSkillName(bossSkills[num]);

            DoCurrentSkill();
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

            DoCurrentSkill();
        }
        catch (IndexOutOfRangeException)
        {
            Debug.LogError(num + " is not Match for " + gameObject.name);
        }
        catch (Exception)
        {
            Debug.LogError("Error in SetSkillNum of " + gameObject.name);
        }
    }
    private void DebugBossSkillName(IBossSkill b)
    {
        Debug.Log("Current Skill is '" + b.GetSkillScriptName() + "'.");
    }
}
