using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStat : EnemyStatus
{
    public SearchCharacter searchCharacter { get; private set; }
    public Status currentStatus { get; private set; }

    [Header("공중유닛인가?")]
    [SerializeField]
    private bool _isAirEnemy = false;
    public bool isAirEnemy
    {
        get { return _isAirEnemy; }
    }

    [Header("적유닛 스탯관련")]
    [SerializeField]
    private float _hp = 3f;
    public float hp
    {
        get { return _hp; }
        set { _hp = value; }
    }
    [SerializeField]
    private float _ap = 1f;
    public float ap
    {
        get { return _ap; }
        set { _ap = value; }
    }
    [SerializeField]
    private float _dp = 1f;
    public float dp
    {
        get { return _dp; }
        set { _dp = value; }
    }
    [SerializeField]
    private float _searchSpeed = 1f;
    public float searchSpeed
    {
        get { return _searchSpeed; }
    }
    [SerializeField]
    private float _pursueSpeed = 1f;
    public float pursueSpeed
    {
        get { return _pursueSpeed; }
    }
    [SerializeField]
    private float _attackDelay = 1f;
    public float attackDelay
    {
        get { return _attackDelay; }
    }

    [Header("스테이터스 변경 관련 함수")]
    [SerializeField]
    private float _foundRange = 5f;
    public float foundRange
    {
        get { return _foundRange; }
    }
    [SerializeField]
    private float _attackRange = 1f;
    public float attackRange
    {
        get { return _attackRange; }
    }

    [Header("스테이터스 변경을 위한 플레이어 포지션값")]
    [SerializeField]
    private Transform _playerPosition = null;
    public Transform playerPosition
    {
        get { return _playerPosition; }
    }

    void Start()
    {
        searchCharacter = GetComponent<SearchCharacter>();
    }


    void Update()
    {
        currentStatus = searchCharacter.CheckStatus(playerPosition.position, foundRange, attackRange);
    }
}
