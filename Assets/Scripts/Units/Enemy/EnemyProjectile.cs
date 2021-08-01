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
    public void SpawnSet(bool fX, float shootR, float dm)
    {
        flipX = fX;
        firstPosition = transform.position;
        isDestroy = false;
        shootRange = shootR;
        damage = dm;
    }
    public void Move()
    {
        if (spriteRenderer.flipX)
        {
            transform.Translate(Vector2.left * speed * Time.fixedDeltaTime);
        }
        else
        {
            transform.Translate(Vector2.right * speed * Time.fixedDeltaTime);
        }
    }
    public void GetDamage()
    {
        float distance = Vector2.Distance(gameManager.player.currentPosition, transform.position);

        if (distance <= 0.5f)
        {
            CharacterStat _player = gameManager.player.GetComponent<CharacterStat>();
            CharacterMove _playerMove = gameManager.player.GetComponent<CharacterMove>();

            if (_playerMove.canHurt)
            {
                float p_hp = _player.hp;
                float p_dp = _player.dp;

                float totalDamage;

                totalDamage = damage - p_dp;

                if (totalDamage <= 0f)
                {
                    totalDamage = 0.5f;
                }

                p_hp -= totalDamage;
                _player.hp = p_hp;

                _playerMove._Hurt();
            }
            isDestroy = true;
            stageManager.DespawnProjectile(gameObject);
        }
    }
    public void Despawn()
    {
        if (!isDestroy)
        {
            float distance = Vector2.Distance(firstPosition, transform.position);

            if (distance >= shootRange)
            {
                isDestroy = true;
                stageManager.DespawnProjectile(gameObject);
            }
        }
    }
}
