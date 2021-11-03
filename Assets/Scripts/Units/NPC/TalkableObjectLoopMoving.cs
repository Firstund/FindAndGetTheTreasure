using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkableObjectLoopMoving : MonoBehaviour
{
    private GameManager gameManager = null;
    private ObjectOutCheck objectOutCheck = null;

    [SerializeField]
    private Vector2 moveRange;

    [SerializeField]
    private float moveSpeed = 1f;

    private Vector2 originPos = Vector2.zero;
    private Vector2 targetPos = Vector2.zero;

    private bool moveToTargetPos = true;

    private bool MoveToTargetPos
    {
        get { return moveToTargetPos; }
        set
        {
            moveToTargetPos = value;
        }
    }

    private SpriteRenderer spriteRenderer = null;
    private Animator anim = null;

    private Vector2 currentPosition = Vector2.zero;

    void Start()
    {
        gameManager = GameManager.Instance;

        originPos = transform.position;
        targetPos = originPos + moveRange;

        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        DirectionCheck();

        objectOutCheck = GetComponent<ObjectOutCheck>();

        if (objectOutCheck == null)
        {
            objectOutCheck = gameObject.AddComponent<ObjectOutCheck>();

            Debug.LogWarning(gameObject.name + "has no ObjectOutCheck Script.");
        }
    }

    private void DirectionCheck()
    {
        if (moveRange.x > 0f)
        {
            spriteRenderer.flipX = false;
        }
        else
        {
            spriteRenderer.flipX = true;
        }
    }

    void Update()
    {
        if (!objectOutCheck.IsOutCamera)
        {
            currentPosition = transform.position;

            if (!gameManager.stopTime)
            {
                anim.speed = 1f;
                Move();
            }
            else
            {
                anim.speed = 0f;
            }

            transform.position = currentPosition;
        }
    }

    private void Move()
    {
        float distance = 0f;

        if (moveToTargetPos)
        {
            currentPosition = Vector2.MoveTowards(currentPosition, targetPos, moveSpeed * Time.deltaTime);

            distance = Vector2.Distance(currentPosition, targetPos);

            if (distance <= 0.1f)
            {
                moveToTargetPos = false;
                spriteRenderer.flipX = !spriteRenderer.flipX;
            }
        }
        else
        {
            currentPosition = Vector2.MoveTowards(currentPosition, originPos, moveSpeed * Time.deltaTime);

            distance = Vector2.Distance(currentPosition, originPos);

            if (distance <= 0.1f)
            {
                moveToTargetPos = true;
                spriteRenderer.flipX = !spriteRenderer.flipX;
            }
        }
    }
    private void OnDrawGizmosSelected() 
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position.Sum(moveRange));
    }
}
