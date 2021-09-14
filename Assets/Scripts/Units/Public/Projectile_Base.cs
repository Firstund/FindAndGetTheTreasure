using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_Base : MonoBehaviour
{
    protected GameManager gameManager = null;
    protected StageManager stageManager = null;
    protected SpriteRenderer spriteRenderer = null;

    [SerializeField]
    protected float speed = 1f;
    protected float originSpeed = 0f;
    [SerializeField]
    protected float damage = 1f;
    [SerializeField]
    protected float hitRange = 0.5f;
    protected float shootRange = 0f;
    protected bool isDestroy = false;

    protected Vector2 firstPosition = Vector2.zero;
    protected Vector2 shootAngle = Vector2.zero;

    protected void Start()
    {
        gameManager = GameManager.Instance;
        stageManager = FindObjectOfType<StageManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        originSpeed = speed;
    }
    // 벽에 부딪히면 사라지는 코드
}
