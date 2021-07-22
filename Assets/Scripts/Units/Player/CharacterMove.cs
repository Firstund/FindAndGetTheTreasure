using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CharacterMove : MonoBehaviour
{
    private string name;
    private Rigidbody2D rigid = null;
    private BoxCollider2D boxCol2D = null;
    private Animator anim = null;
    public SpriteRenderer spriteRenderer { get; private set; }

    private PlayerInput playerInput = null;
    private SpawnAfterImage spawnAfterImage = null;
    private CharacterStat characterStat = null;

    [SerializeField]
    private LayerMask whatIsGround;
    [SerializeField]
    private Transform GroundChecker;
    private bool isGround = false;

    [SerializeField]
    private float dashRange = 2f;
    [SerializeField]
    private float dashStopRange = 0.1f;
    [SerializeField]
    private float dashDoTime = 0.1f;
    [SerializeField]
    private float dashResetTime = 1f;
    private bool isJump = false;
    private bool isDash = false;
    private bool isAttack = false;
    private bool canDash = true;
    private bool canDashAttack = true;
    private bool dashMoving = false;
    private bool staping = false;
    private bool attacking = false;
    private bool canSpawnAfterImage = true;

    private Vector2 dashPosition = Vector2.zero;

    public Vector2 currentPosition { get; private set; }
    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        boxCol2D = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        playerInput = GetComponent<PlayerInput>();
        characterStat = GetComponent<CharacterStat>();
        spawnAfterImage = GetComponent<SpawnAfterImage>();
    }

    void Start()
    {
        if (playerInput == null)
        {
            playerInput = gameObject.AddComponent<PlayerInput>();
        }

        name = characterStat.name;
    }
    void Update()
    {

        if (playerInput.isJump)
        {
            attacking = false;
            isJump = true;
        }

        if (playerInput.isDash)
        {
            isDash = true;
        }

        if (playerInput.isAttack && !attacking)
        {
            isAttack = true;
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
        InAirCheck();
        Dash(XMove);

        Attack();

        DashMove();
        SpawnAfterImage();

        transform.position = currentPosition;
    }
    private void Dash(float XMove)
    {
        if (!(XMove == 0) && isDash && !dashMoving && canDash)
        {
            float _dashRange = dashRange;
            dashPosition = currentPosition;  

            if (attacking)
            {
                if (canDashAttack)
                {
                    anim.Play(name + "DashAttack");

                    _dashRange = dashRange / 2;

                    dashPosition.x = currentPosition.x + _dashRange;
                }
            }
            else
            {
                canDashAttack = false;
                attacking = false;
            }

            if (spriteRenderer.flipX)
            {
                _dashRange = -_dashRange;
            }

            dashPosition.x = currentPosition.x + _dashRange;
            dashMoving = true;

            isDash = false;
            canDash = false;

            

            Invoke("DashRe", dashResetTime);
        }
        else
        {
            isDash = false;
        }

    }
    private void DashRe()
    {
        canDash = true;
        canDashAttack = true;
    }
    private void SpawnAfterImage()
    {
        if (dashMoving && canSpawnAfterImage)
        {
            float spawnAfterImageDelay = Random.Range(spawnAfterImage.spawnAfterImageDelayMinimum, spawnAfterImage.spawnAfterImageDelayMaximum);
            spawnAfterImage.SetAfterImage();
            canSpawnAfterImage = false;

            Invoke("SpawnAfterImageRe", spawnAfterImageDelay);
        }

    }
    private void SpawnAfterImageRe()
    {
        canSpawnAfterImage = true;
    }
    private void DashMove()
    {
        if (dashMoving)
        {
            transform.DOMove(dashPosition, dashDoTime).SetEase(Ease.InQuad);
        }

        Vector2 _dashPosition = dashPosition;
        Vector2 _currentPosition = currentPosition;
        _dashPosition.y = 0f;
        _currentPosition.y = 0f;

        float distance = Vector2.Distance(_dashPosition, _currentPosition);


        if (distance <= dashStopRange)
        {
            dashMoving = false;
        }
    }
    private void Attack()
    {
        if (!attacking && !dashMoving && !staping && isAttack) //isGround에 따라서 GroundAttack과 InAirAttack을 나눌것, dashing == true라면 dashAttack을 할것
        {
            if (isGround)
            {
                attacking = true;

                anim.Play(name + "Attack");

                isAttack = false;
            }
            else if (!isGround)
            {
                attacking = true;
                anim.Play(name + "InAirAttack");

                isAttack = false;
            }
        }
    }
    private void SetAttacking()
    {
        attacking = false;
        isAttack = false;
    }
    private void Jump()
    {
        if (isJump && !staping && !attacking && isGround)
        {
            anim.Play(name + "Jump");
        }
    }
    public void Jumping()
    {
        rigid.AddForce(Vector2.up * characterStat.jumpSpeed, ForceMode2D.Impulse);
        isJump = false;
    }
    private void InAirCheck()
    {
        if (!isGround && !staping && !attacking)
        {
            anim.Play(name + "InAir");
        }
    }

    private void MoveX(float XMove)
    {
        rigid.velocity = new Vector2(XMove * characterStat.speed, rigid.velocity.y);
    }

    private void GroundCheck()
    {
        bool a = Physics2D.OverlapCircle(GroundChecker.position, 0.1f, whatIsGround);

        if (isGround == false && a) // 착지하는 순간
        {
            SetAttacking();
            staping = true;
            anim.Play(name + "Stap");
        }

        isGround = a;
    }
    private void SetStaping()
    {
        staping = false;
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

        if (!isJump && !staping && !attacking && isGround)
        {
            attacking = false;
            
            if (XMove != 0f)
            {   
                anim.Play(name + "Run");
            }
            else
            {
                anim.Play(name + "Idle");
            }
        }
    }
}
