using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : Projectile_Base, IProjectile
{
    void Update()
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
        transform.Translate(shootAngle * speed * Time.fixedDeltaTime);
    }
    public void GetDamage()
    {
        float distance = 0f;

        foreach(var item in stageManager.Enemys)
        {
            if(item.activeSelf)
            {
                distance = Vector2.Distance(item.transform.position, transform.position);

                if(distance <= 0.5f)
                {
                    EnemyMove enemyMove = item.GetComponent<EnemyMove>();

                    enemyMove.Hurt(damage);

                    isDestroy = true;
                    stageManager.DespawnProjectile(gameObject);
                }
            }
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
                Debug.Log(firstPosition);

                stageManager.DespawnProjectile(gameObject);
            }
        }
    }
}
