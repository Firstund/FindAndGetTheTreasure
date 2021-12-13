using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawnObjects : BossSpawnObjectsBase
{
    [SerializeField]
    private GameObject spawnIt = null;

    [SerializeField]
    private SpawnInfos spawnInfos = new SpawnInfos();
    [SerializeField]
    private RandomSpawnInfos randomSpawnInfos = new RandomSpawnInfos();
    [SerializeField]
    private AngleSpawnInfos angleSpawnInfos = new AngleSpawnInfos();

    private Vector2 targetPos = Vector2.zero;
    private Vector2 firstTargetPos = Vector2.zero;

    private Vector2 shootDir = Vector2.right;

    private float angle = 0f;
    private float spawnTimer = 0f;

    private int spawnCount = 0;

    private bool spawnStart = false;

    private void Awake()
    {
        base.DoAwake();
    }
    private void Update()
    {
        if (spawnStart)
        {
            if (spawnTimer >= spawnInfos.spawnDelay && spawnCount < spawnInfos.spawnNum)
            {
                Spawn();

                spawnTimer = 0f;
                spawnCount++;
            }
            else
            {
                spawnTimer += Time.deltaTime;
            }
        }
    }

    public override string GetSkillScriptName()
    {
        return "BossSpawnObjects";
    }
    public override void DoSkill()
    {
        if (doThisSkill)
        {
            if (spawnInfos.spawnDistance < bossStatus.DistanceWithPlayer)
            {
                doThisSkill = false;
                bossStatus.DoCurrentSkillFail();

                return;
            }

            base.DoSkill();

            targetPos = transform.position;
            firstTargetPos = targetPos;

            spawnCount = 0;
            spawnTimer = spawnInfos.spawnDelay;

            SpawnSet();
        }
    }
    private void SpawnStart()
    {
        spawnStart = true;
    }
    private void Spawn()
    {
        if (doThisSkill && spawnStart)
        {
            DoSpawn(angle);
            SpawnDoneSet();
        }
    }

    private void SpawnDoneSet()
    {
        if (randomSpawnInfos.randomSetSpawnPos)
        {
            targetPos = firstTargetPos;
            targetPos += ScriptHelper.RandomVector(randomSpawnInfos.minDis, randomSpawnInfos.maxDis);
        }

        if (angleSpawnInfos.angleSpawnPos)
        {
            angle += angleSpawnInfos.anglePlus;
        }

        if (spawnCount == spawnInfos.spawnNum - 1)
        {
            doThisSkill = false;
            spawnStart = false;

            bossStatus.DoCurrentSkillSuccess();
        }
    }

    private void SpawnSet()
    {
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
    }

    private void DoSpawn(float angle)
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
    }
}
