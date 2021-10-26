using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public float XMove { get; private set; }
    public bool isJump { get; private set; }
    public bool isDash { get; private set; }
    public bool isAttack { get; set; }
    public bool moveBtnDown { get; private set; }
    public bool spawnPortal0 { get; private set; }
    public bool spawnPortal1 { get; private set; }
    public bool usePortal { get; private set; }
    public bool timeWarp { get; private set; }

    // Update is called once per frame
    void Update()
    {
        XMove = Input.GetAxisRaw("Horizontal");
        isJump = Input.GetButtonDown("Jump");
        isDash = Input.GetButtonDown("Dash");
        isAttack = Input.GetButtonDown("Attack");
        moveBtnDown = Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow);
        spawnPortal0 = Input.GetButtonDown("SpawnPortal0");
        spawnPortal1 = Input.GetButtonDown("SpawnPortal1");
        usePortal = Input.GetButtonDown("UsePortal");
        timeWarp = Input.GetButtonDown("TimeWarp");
    }
}
