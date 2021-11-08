using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : Projectile_Base, IProjectile
{
    //TODO: 쏘면 플레레이어의 현재 위치로 날아가는 위치를 고정할 것인가에 관한 bool 변수와, 플레이어의 현재위치를 날아갈 방향으로 정하는 코드 작성
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
        Move();
        Despawn();
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
    public void Move()
    {
        transform.Translate(shootDir * speed * Time.fixedDeltaTime);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
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
