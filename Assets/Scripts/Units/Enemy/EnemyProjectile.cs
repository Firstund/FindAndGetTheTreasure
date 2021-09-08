using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    private GameManager gameManager = null;
    private StageManager stageManager = null;
    private SpriteRenderer spriteRenderer = null;

    [SerializeField]
    private float speed = 1f;
    private float damage = 1f;
    private float shootRange = 0f;
    private bool flipX = false;
    private bool isDestroy = false;

    private Vector2 firstPosition = Vector2.zero;
    private Vector2 shootAngle = Vector2.zero;

    void Start()
    {
        gameManager = GameManager.Instance;
        stageManager = FindObjectOfType<StageManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        spriteRenderer.flipX = flipX;
        Move();
        Despawn();
        GetDamage();
    }
    public void SpawnSet(bool fX, float shootR, float dm, Vector2 angle)
    {
        flipX = fX;
        firstPosition = transform.position;
        isDestroy = false;
        shootRange = shootR;
        damage = dm;
        shootAngle = angle;
    }
    public void Move()
    {
        // if (spriteRenderer.flipX)
        // {
        //     transform.Translate(Vector2.left * speed * Time.fixedDeltaTime);
        // }
        // else
        // {
        //     transform.Translate(Vector2.right * speed * Time.fixedDeltaTime);
        // }

        transform.Translate(shootAngle * speed * Time.fixedDeltaTime);
    }
    public void GetDamage()
    {
        // float distance = Vector2.Distance(gameManager.player.currentPosition, transform.position);

        // if (distance <= 0.5f)
        // {
        //     CharacterMove characterMove = gameManager.player.GetComponent<CharacterMove>();

        //     if (characterMove.canHurt)
        //     {
        //         characterMove.Hurt(damage);
        //     }
            
        //     isDestroy = true;
        //     stageManager.DespawnProjectile(gameObject);
        // }
    }
    public void Despawn()
    {
        // if (!isDestroy)
        // {
        //     float distance = Vector2.Distance(firstPosition, transform.position);

        //     if (distance >= shootRange)
        //     {
        //         isDestroy = true;
                
        //         stageManager.DespawnProjectile(gameObject);
        //     }
        // }
    }
}
