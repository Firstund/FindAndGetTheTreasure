using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSelectCharacter : MonoBehaviour
{
    private PlayerInput playerInput = null;

    private Animator anim = null;
    private Rigidbody2D rigid = null;
    private SpriteRenderer spriteRenderer = null;

    [SerializeField]
    private string characterName = "Umiko";

    [SerializeField]
    private float moveSpeed = 1f;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        playerInput = GetComponent<PlayerInput>();
    }
    void Start()
    {

    }

    void Update()
    {
        LRCheck(playerInput.XMove);
    }
    private void FixedUpdate()
    {
        MoveX(playerInput.XMove);
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

        if (XMove != 0f)
        {
            anim.Play(characterName + "Run");
        }
        else
        {
            anim.Play(characterName + "Idle");
        }
    }
    private void MoveX(float XMove)
    {
        rigid.velocity = new Vector2(moveSpeed * XMove, rigid.velocity.y);
    }
}
