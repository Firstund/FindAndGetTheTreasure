using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : Projectile_Base, IProjectile
{
    [Header("alpha값이 오르는 스피드를 조절해줌")]
    [SerializeField]
    private float alphaPlusSpeed = 1f;
    [Header("이 총알 스프라이트의 알파값이 이정도는 되어야 총알이 움직이기 시작")]
    [SerializeField]
    private float startMoveAlpha = 1f;
    private float currentAlpha = 0f;

    [SerializeField]
    private bool isMoveToPlayer = false;

    void FixedUpdate()
    {
        if (gameManager.SlowTimeSomeObjects && gameManager.CurrentSlowTimePerSlowTime == 0)
        {
            speed = 0f;

            DoFixedUpdate();
        }
        else if (!gameManager.SlowTimeSomeObjects)
        {
            speed = originSpeed;

            DoFixedUpdate();
        }
    }

    private void DoFixedUpdate()
    {
        currentAlpha = spriteRenderer.color.a;

        if (currentAlpha < startMoveAlpha)
        {
            spriteRenderer.color = new Vector4(1f, 1f, 1f, currentAlpha + (Time.fixedDeltaTime * alphaPlusSpeed));
        }
        else
        {
            Move();
            Despawn();
        }
    }

    public void SpawnSet(float shootR, float dm, Vector2 dir)
    {
        firstPosition = transform.position;
        isDestroy = false;
        shootRange = shootR;
        damage = dm;

        if (isMoveToPlayer)
        {
            shootDir = GameManager.Instance.player.currentPosition - (Vector2)transform.position;
            shootDir = shootDir.normalized;
        }
        else
        {
            shootDir = dir;
        }
    }
    public void SpawnSet(float shootR, float dm, Vector2 dir, float a)
    {
        firstPosition = transform.position;
        isDestroy = false;
        shootRange = shootR;
        damage = dm;

        spriteRenderer.color = new Vector4(1f, 1f, 1f, a);

        if (isMoveToPlayer)
        {
            shootDir = GameManager.Instance.player.currentPosition - (Vector2)transform.position;
            shootDir = shootDir.normalized;
        }
        else
        {
            shootDir = dir;
        }
    }
    public void Move()
    {
        transform.Translate(shootDir * speed * Time.fixedDeltaTime);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && currentAlpha >= startMoveAlpha)
        {
            GetDamage();
        }
    }
    public void GetDamage()
    {
        CharacterMove characterMove = gameManager.player.GetComponent<CharacterMove>();

        if (characterMove.canHurt)
        {
            characterMove.Hurt(damage);
        }

        DespawnProjectile();
    }
    public void Despawn()
    {
        if (!isDestroy)
        {
            float distance = 0f;

            distance = Vector2.Distance(firstPosition, transform.position);

            if (distance >= shootRange)
            {
                DespawnProjectile();
            }
        }
    }

    private void DespawnProjectile()
    {
        isDestroy = true;

        stageManager.DespawnProjectile(gameObject);
    }
}
