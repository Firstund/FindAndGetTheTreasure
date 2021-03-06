using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleMove : MonoBehaviour
{
    private GameManager gameManager = null;
    private CharacterMove player = null;

    [Header("현재 위치에서 얼마나 움직일 것인가")]
    [SerializeField]
    private Vector2 targetPos = Vector2.zero;
    private Vector2 originPos = Vector2.zero;

    [SerializeField]
    private float moveSpeed = 0f;
    private float moveTime = 0f;
    private float pasteMoveTimer = 0f;
    private float moveTimer = 0f;
    public Vector2 changeValue { get; private set; }

    private bool crashedWithPlayer = false;
    private bool moveToTarget = false;

    void Start()
    {
        gameManager = GameManager.Instance;
        player = gameManager.player;
        originPos = transform.position;
        targetPos += originPos;

        moveTime = Vector2.Distance(originPos, targetPos) / moveSpeed;
    }

    void FixedUpdate()
    {
        // GameManager에 의한 시간에 종속될것
        if (!gameManager.stopTime)
        {
            if (gameManager.SlowTimeSomeObjects && gameManager.CurrentSlowTimePerSlowTime == 0)
            {
                DoFixedUpdate();
            }
            else if (!gameManager.SlowTimeSomeObjects)
            {
                DoFixedUpdate();
            }
        }
    }

    private void DoFixedUpdate()
    {
        Vector2 newPos = Vector2.zero;

        if (moveToTarget)
        {
            if (moveTimer > 0f)
            {
                moveTimer -= Time.fixedDeltaTime;
            }
            else
            {
                moveToTarget = false;
            }
        }
        else
        {
            if (moveTimer < moveTime)
            {
                moveTimer += Time.fixedDeltaTime;
            }
            else
            {
                moveToTarget = true;
            }
        }

        newPos = Vector2.Lerp(originPos, targetPos, moveTimer / moveTime);
        transform.position = newPos;

        changeValue = newPos - Vector2.Lerp(originPos, targetPos, pasteMoveTimer / moveTime);

        if (crashedWithPlayer)
        {
            player.transform.position = player.transform.position.Sum(changeValue);
        }

        pasteMoveTimer = moveTimer;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (1 << other.gameObject.layer == LayerMask.GetMask("PLAYER"))
        {
            crashedWithPlayer = true;
        }
    }
    private void OnCollisionExit2D(Collision2D other)
    {
        if (1 << other.gameObject.layer == LayerMask.GetMask("PLAYER"))
        {
            crashedWithPlayer = false;
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position.Sum(targetPos));
    }
}
