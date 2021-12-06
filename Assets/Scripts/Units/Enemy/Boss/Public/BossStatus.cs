using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStatus : MonoBehaviour
{
    private GameManager gameManager = null;
    private StageManager stageManager = null;

    private SpriteRenderer spriteRenderer = null;
    private Rigidbody2D rigid = null;
    private Animator anim = null;
    public Animator Anim
    {
        get { return anim; }
    }

    private BossStat bossStat = null;

    private List<BossSkillBase> bossSkills = new List<BossSkillBase>();
    private List<int> ignoreBossSkillNums = new List<int>(); // DoSkill함수 실행에 실패한 스킬들의 모임

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

    private float firstAnimSpeed = 0f;
    private float firstMass = 0f;
    private float firstGravity = 0f;

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
        rigid = GetComponent<Rigidbody2D>();

        bossStat = GetComponent<BossStat>();

        bossSkills = GetComponents<BossSkillBase>().ToList();
    }
    private void Start()
    {
        RandomSetSkillNum();
        DoCurrentSkill();

        firstAnimSpeed = anim.speed;
        firstMass = rigid.mass;
        firstGravity = rigid.gravityScale;

        bossStat.WhenIsDead += () =>
        {
            for (int i = 0; i < bossSkills.Count; i++)
            {
                bossSkills[i].DoThisSkill = false;
            }
        };
    }
    private void FixedUpdate() 
    {
        if(gameManager.SlowTimeSomeObjects)
        {
            anim.speed = firstAnimSpeed / gameManager.SlowTimeNum;
            rigid.mass = firstMass / gameManager.SlowTimeNum;
            rigid.gravityScale = firstGravity / gameManager.SlowTimeNum;
        }
        else if(gameManager.stopTime)
        {
            anim.speed = 0f;
            rigid.mass = 0f;
            rigid.gravityScale = 0f;
        }
        else
        {
            anim.speed = firstAnimSpeed;
            rigid.mass = firstMass;
            rigid.gravityScale = firstGravity;
        }
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
        for (int i = 0; i < bossSkills.Count; i++)
        {
            if (bossSkills[i].DoThisSkill) // 이미 실행중인 스킬이 있으면, 이 스킬은 실행하지 않는다.
            {
                return;
            }
        }

        Debug.Log("Do " + bossSkills[currentSkillNum].GetSkillScriptName() + ".");

        bossSkills[currentSkillNum].DoThisSkill = true;
        bossSkills[currentSkillNum].DoSkill();
    }
    public void DoCurrentSkillFail() // 현재 스킬 실행에 실패했을 때 실행.
    {
        Debug.Log("Do " + bossSkills[currentSkillNum].GetSkillScriptName() + " failed.");

        ignoreBossSkillNums.Add(currentSkillNum);

        RandomSetSkillNum();

        DoCurrentSkill();
    }
    public void DoCurrentSkillSuccess() // 현재 스킬이 성공적으로 실행되었을 때 실행.
    {
        Debug.Log("Do " + bossSkills[currentSkillNum].GetSkillScriptName() + " successed.");

        ignoreBossSkillNums.Add(currentSkillNum);

        RandomSetSkillNum();

        Invoke("DoSkillAgain", doSkillCycle);
    }
    private void DoSkillAgain()
    {
        DoCurrentSkill();
    }
    public void ClearFailedBossSkillNumList()
    {
        Debug.Log("Clear List");
        ignoreBossSkillNums.Clear();
    }
    public void RandomSetSkillNum()
    {
        if (bossSkills.Count > 0)
        {
            int num;

            while (true)
            {
                bool numSetAgain = false;

                if (ignoreBossSkillNums.Count == bossSkills.Count)
                {
                    ignoreBossSkillNums.Clear();
                }

                num = UnityEngine.Random.Range(0, bossSkills.Count);

                for (int i = 0; i < ignoreBossSkillNums.Count; i++)
                {
                    if (ignoreBossSkillNums[i] == num)
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
            Debug.LogError("Error in SetSkillNum of " + gameObject.name);
        }
    }
    private void DebugBossSkillName(IBossSkill b)
    {
        Debug.Log("Current Skill is '" + b.GetSkillScriptName() + "'.");
    }
}
