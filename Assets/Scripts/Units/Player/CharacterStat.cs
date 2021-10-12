using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStat : MonoBehaviour
{
    private GameManager gameManager = null;

    [Header("캐릭터의 이름")]
    [SerializeField]
    private string _characterName = "";
    public string characterName
    {
        get { return _characterName; }
    }
    [Header("캐릭터의 능력치 관련")]
    [SerializeField]
    private float _hp = 0f;
    public float hp
    {
        get { return _hp; }
        set { _hp = value; }
    }

    [SerializeField]
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
    private float _ap = 0f;
    public float ap
    {
        get { return _ap; }
        set { _ap = value; }
    }
    [SerializeField]
    private float _dp = 0f;
    public float dp
    {
        get { return _dp; }
        set { _dp = value; }
    }
    [SerializeField]    // 대쉬, 대쉬어택등 다양한 특수 스킬 관련 에너지, 에너지는 꽤 빠른 속도로 차오른다.
    private float _sp = 0f;
    public float sp
    {
        get { return _sp; }
        set { _sp = value; }
    }
    [SerializeField]
    private float _firstSp = 0f;
    public float firstSp
    {
        get { return _firstSp; }
        set { _firstSp = value; }
    }
    [SerializeField]
    private float _spReSpeedPerSec = 1f;
    public float spReSpeedPerSec
    {
        get { return _spReSpeedPerSec; }
        set { _spReSpeedPerSec = value; }
    }
    [SerializeField]
    private float _attackRange = 0.5f;
    public float attackRange
    {
        get { return _attackRange; }
    }

    [Header("Speed관련")]
    [SerializeField]
    private float _speed = 0f;
    public float speed
    {
        get { return _speed; }
    }
    [SerializeField]
    private float _hangSpeed = 0f;
    public float hangSpeed
    {
        get { return _hangSpeed; }
    }
    [SerializeField]
    private float _jumpSpeed = 0f;
    public float jumpSpeed
    {
        get { return _jumpSpeed; }
    }

    private void Start()
    {
        _firstHp = hp;
        _firstSp = sp;

        gameManager = GameManager.Instance;
        gameManager.characterStat = this;
    }
    private void Update()
    {
        if (!(gameManager.SlowTimeSomeObjects || gameManager.stopTime))
        {
            if (sp < firstSp)
            {
                sp += spReSpeedPerSec * Time.deltaTime;
            }
            else
            {
                sp = firstSp;
            }
        }
    }
}
