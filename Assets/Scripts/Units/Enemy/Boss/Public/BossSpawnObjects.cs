using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawnObjects : BossSpawnObjectsBase, IBossSkill
{
    private GameManager gameManager = null;
    private StageManager stageManager = null;

    private BossStatus bossStatus = null;

    [SerializeField]
    private GameObject spawnIt = null;

    [SerializeField]
    private SpawnInfos spawnInfos = new SpawnInfos();
    [SerializeField]
    private RandomSpawnInfos randomSpawnInfos = new RandomSpawnInfos();
    [SerializeField]
    private AngleSpawnInfos angleSpawnInfos = new AngleSpawnInfos();

    [Header("이 스킬은 다음 스킬이 들어올 때 까지 반복하는가")]
    [SerializeField]
    private bool spawnLooping = false;

    private void Awake()
    {
        gameManager = GameManager.Instance;
        stageManager = StageManager.Instance;

        bossStatus = GetComponent<BossStatus>();
    }

    public string GetSkillScriptName()
    {
        return "BossSpawnObjects";
    }
    public void DoSkill()
    {
        if (spawnInfos.spawnDistance < bossStatus.DistanceWithPlayer)
        {
            bossStatus.DoCurrentSkillFail();

            return;
        }

        bossStatus.Anim.SetTrigger("Skill1");
    }
    private void DoSpawn()
    {
        StartCoroutine(Spawn());
    }
    private IEnumerator Spawn()
    {
        Vector2 targetPos = transform.position;
        Vector2 firstTargetPos = targetPos;

        Vector2 shootDir = Vector2.right;

        float angle = 0f;

        if (randomSpawnInfos.randomSetSpawnPos)
        {
            targetPos += ScriptHelper.RandomVector(randomSpawnInfos.minDis, randomSpawnInfos.maxDis);
        }
        else if (spawnInfos.spawnPos.position != Vector3.zero)
        {
            targetPos = spawnInfos.spawnPos.position;
            firstTargetPos = targetPos;
        }

        if (angleSpawnInfos.angleSpawnPos && spawnInfos.isProjectile)
        {
            angle = angleSpawnInfos.startAngle;
        }

        for (int i = 0; i < spawnInfos.spawnNum; i++)
        {
            if (spawnInfos.isProjectile)
            {
                if (spawnInfos.shootToPlayer)
                {
                    shootDir = (gameManager.player.transform.position - transform.position).normalized;
                }

                stageManager.ShootProjectile(spawnIt, spawnInfos.projectileDamage, targetPos, Quaternion.Euler(0f, 0f, angle), shootDir, spawnInfos.spawnDistance, spawnInfos.spawnAlpha);
            }
            else
            {
                stageManager.SpawnEnemy(spawnIt, targetPos);
            }

            yield return new WaitForSeconds(spawnInfos.spawnDelay);

            if (randomSpawnInfos.randomSetSpawnPos)
            {
                targetPos = firstTargetPos;
                targetPos += ScriptHelper.RandomVector(randomSpawnInfos.minDis, randomSpawnInfos.maxDis);
            }

            if (angleSpawnInfos.angleSpawnPos)
            {
                angle += angleSpawnInfos.anglePlus;
            }

            if (i == spawnInfos.spawnNum - 1)
            {
                bossStatus.DoCurrentSkillSuccess();

                if (!spawnLooping)
                {
                    bossStatus.Anim.SetTrigger("Idle");
                }
            }
        }
    }

}
