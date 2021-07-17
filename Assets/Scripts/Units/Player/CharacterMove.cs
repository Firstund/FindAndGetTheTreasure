using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMove : MonoBehaviour
{
    private Rigidbody2D rigid = null;
    private BoxCollider2D boxCol2D = null;
    private SpriteRenderer spriteRenderer = null;
    private PlayerInput playerInput = null;

    [SerializeField]
    private LayerMask whatIsGround;
    [SerializeField]
    private Transform GroundChecker;
    private bool isGround = false;

    [SerializeField]
    private float speed = 2f;
    [SerializeField]
    private float jumpSpeed = 2f;
    private bool isJump = false;
    private bool isDash = false;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        boxCol2D = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        playerInput = GetComponent<PlayerInput>();

        if (playerInput == null)
        {
            playerInput = gameObject.AddComponent<PlayerInput>();
        }
    }
    void Update()
    {
        if(playerInput.isJump)
        {
            isJump = true;
        }

        if(playerInput.isDash)
        {
            isDash = true;
        }
    }
    void FixedUpdate()
    {
        float XMove = playerInput.XMove;

        LRCheck(XMove);

        GroundCheck();

        MoveX(XMove);

        Jump();
    }

    private void Jump()
    {
        if (isJump && isGround)
        {
            rigid.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
            isJump = false;
        }
    }

    private void MoveX(float XMove)
    {
        rigid.velocity = new Vector2(XMove * speed, rigid.velocity.y);
    }

    private void GroundCheck()
    {
        isGround = Physics2D.OverlapCircle(GroundChecker.position, 0.1f, whatIsGround);
    }

    private void LRCheck(float XMove)
    {
        if (XMove == -1f)
        {
            spriteRenderer.flipX = true;
        }
        else if (XMove == 1f)
        {
            spriteRenderer.flipX = false;
        }
    }
}
