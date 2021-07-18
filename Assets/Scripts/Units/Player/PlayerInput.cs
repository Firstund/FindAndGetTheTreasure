using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public float XMove {get; private set;}
    public bool isJump {get; private set;}
    public bool isDash {get; private set;}
    public bool isAttack {get; private set;}

    // Update is called once per frame
    void Update()
    {
        XMove = Input.GetAxisRaw("Horizontal");
        isJump = Input.GetButtonDown("Jump");
        isDash = Input.GetButtonDown("Dash");
        isAttack = Input.GetButtonDown("Attack");
    }
}
