using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStat : EnemyStatus, IDespawnableByOutCamera
{
    private GameManager gameManager = null;
    private StageManager stageManager = null;
    private ObjectOutCheck objectOutCheck = null;
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
    private float _firstHp = 0f;
    public float firstHp
    {
        get
        {
            if (_firstHp == 0f)
            {
                _firstHp = hp;
            }
            return _firstHp;
        }
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
    private bool _isShootProjectile = false;
    public bool isShootProjectile
    {
        get { return _isShootProjectile; }
    }

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
    [SerializeField]
    private float _shootRange = 5f;
    public float shootRange
    {
        get { return _shootRange; }
    }

    private Transform _playerPosition = null; // 스테이터스 변경을 위한 플레이어 포지션값
    public Transform playerPosition
    {
        get { return _playerPosition; }
    }

    [SerializeField]
    private float despawnTimer = 10f;
    private float firstDespawnTimer = 0f;
    public bool IsOutCamera
    {
        get { return objectOutCheck.IsOutCamera; }
    }

    private void Awake()
    {
        searchCharacter = GetComponent<SearchCharacter>();

        gameManager = GameManager.Instance;

        _playerPosition = gameManager.player.transform;

        firstDespawnTimer = despawnTimer;
    }
    void Start()
    {
        stageManager = StageManager.Instance;

        objectOutCheck = GetComponent<ObjectOutCheck>();

        if(objectOutCheck == null)
        {
            objectOutCheck = gameObject.AddComponent<ObjectOutCheck>();
            
            Debug.LogWarning(gameObject.name + "has no ObjectOutCheck Script.");
        }
    }

    void Update()
    {
        currentStatus = searchCharacter.CheckStatus(playerPosition.position, isShootProjectile, foundRange, shootRange, attackRange);
    }

    void FixedUpdate()
    {
        DespawnByOutCamera();
    }

    public void DespawnByOutCamera()
    {
        if (objectOutCheck.IsOutCamera)
        {
            despawnTimer -= Time.fixedDeltaTime;
        }

        if (despawnTimer <= 0f)
        {
            stageManager.DespawnEnemy(gameObject);
        }
    }
}
