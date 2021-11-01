using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleMove : MonoBehaviour
{
    [Header("현재 위치에서 얼마나 움직일 것인가")]
    [SerializeField]
    private Vector2 targetPos = Vector2.zero;
    private Vector2 originPos = Vector2.zero;

    [SerializeField]
    private float moveSpeed = 0f;
    private float moveTime = 0f;
    private float moveTimer = 0f;

    private bool moveToTarget = false;

    void Start()
    {
        originPos = transform.position;
        targetPos += originPos;

        moveTime = Vector2.Distance(originPos, targetPos) / moveSpeed;
    }

    void Update()
    {
        if(moveToTarget)
        {
            if(moveTimer > 0f)
            {
                moveTimer -= Time.deltaTime;
            }
            else
            {
                moveToTarget = false;
            }
        }
        else
        {
            if(moveTimer < moveTime)
            {
                moveTimer += Time.deltaTime;
            }
            else
            {
                moveToTarget = true;
            }
        }

        transform.position = Vector2.Lerp(originPos, targetPos, moveTimer / moveTime);
    }
}
