using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawnObjectsBase : BossSkillBase
{
    [Serializable]
    protected class SpawnInfos
    {
        public Transform spawnPos = null;

        public int spawnNum = 1;

        [Header("이것은 발사체인가?")]
        public bool isProjectile = false;
        [Header("이것이 발사체일 때 플레이어의 현재 위치로 발사하는가?")]
        public bool shootToPlayer = false; 

        [Header("이것이 발사체일 때의 데미지")]
        public float projectileDamage = 1f;
        public float spawnDistance = 10f;
        [Header("소환한 오브젝트에 적용될 첫 알파값")]
        public float spawnAlpha = 1f;
        public float spawnDelay = 1f;
    }
    [Serializable]
    protected class RandomSpawnInfos
    {
        [Header("이 값이 true면 투사체를 발사 할 때 마다 발사위치를 랜덤으로 설정한다.")]
        public bool randomSetSpawnPos = false;

        public Vector2 minDis = new Vector2(1f, 1f);
        public Vector2 maxDis = new Vector2(10f, 10f);
    }
    [Serializable]
    protected class AngleSpawnInfos
    {
        [Header("이 값이 true면 투사체를 발사할 때 마다 발사방향을 일정 값만큼 기울인다.")]
        public bool angleSpawnPos = false;

        [Header("시작 기울임 수치")]
        public float startAngle = 0f;
        [Header("발사할 때마다 기울일 수치")]
        public float anglePlus = 50f;
    }
}
