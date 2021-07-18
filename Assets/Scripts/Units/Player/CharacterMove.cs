using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

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
    [SerializeField]
    private float dashRange = 2f;
    [SerializeField]
    private float dashStopRange = 0.1f;
    [SerializeField]
    private float dashDoTime = 0.1f;
    private bool isJump = false;
    private bool isDash = false;
    private bool dashMoving = false;

    private Vector2 currentPosition = Vector2.zero;
    private Vector2 dashPosition = Vector2.zero;


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

        if (playerInput.isJump)
        {
            isJump = true;
        }

        if (playerInput.isDash)
        {
            isDash = true;
        }

    }
    void FixedUpdate()
    {
        currentPosition = transform.position;

        float XMove = playerInput.XMove;

        LRCheck(XMove);

        GroundCheck();

        MoveX(XMove);

        Jump();
        Dash();
        DashMove();

        transform.position = currentPosition;
    }
    private void Dash()
    {
        if (isDash && !dashMoving)
        {
            dashPosition = currentPosition;

            float _dashRange = dashRange;

            if (spriteRenderer.flipX)
            {
                _dashRange = -_dashRange;
            }

            dashPosition.x += _dashRange;
            dashMoving = true;

            isDash = false;
        }
    }
    private void DashMove()
    {
        if(dashMoving)
        {
            transform.DOMove(dashPosition, dashDoTime).SetEase(Ease.InQuad);
        }

        Vector2 _dashPosition = dashPosition;
        Vector2 _currentPosition = currentPosition;
        _dashPosition.y = 0f;
        _currentPosition.y = 0f;

        float distance = Vector2.Distance(_dashPosition, _currentPosition);

        if(distance <= dashStopRange)
        {
            dashMoving = false;
        }
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
        if (XMove < 0f)
        {
            spriteRenderer.flipX = true;
        }
        else if (XMove > 0f)
        {
            spriteRenderer.flipX = false;
        }
    }
}
