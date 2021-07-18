using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStat : MonoBehaviour
{
    [Header("캐릭터의 이름")]
    [SerializeField]
    private string _name = "";
    public string name
    {
        get{return _name;}
    }
    [Header("캐릭터의 능력치 관련")]
    [SerializeField]
    private float _hp = 0f;
    public float hp{
        get {return _hp;}
    }
    [SerializeField]
    private float _ap = 0f;
    public float ap{
        get {return _ap;}
    }
    [SerializeField]
    private float _dp = 0f;
    public float dp{
        get {return _dp;}
    }

    [Header("Speed관련")]
    [SerializeField]
    private float _speed = 0f;
    public float speed{
        get {return _speed;}
    }
    [SerializeField]
    private float _jumpSpeed = 0f;
    public float jumpSpeed{
        get {return _jumpSpeed;}
    }
}
