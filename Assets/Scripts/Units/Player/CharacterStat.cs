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
        get { return _firstHp; }
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

        gameManager = GameManager.Instance;
        gameManager.characterStat = this;
    }
}
