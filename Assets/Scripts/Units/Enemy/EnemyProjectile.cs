using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : Projectile_Base, IProjectile
{
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
        transform.Translate(shootAngle * speed * Time.fixedDeltaTime);
    }
    public void GetDamage()
    {
        float distance = Vector2.Distance(gameManager.player.currentPosition, transform.position);

        if (distance <= 0.5f)
        {
            CharacterMove characterMove = gameManager.player.GetComponent<CharacterMove>();

            if (characterMove.canHurt)
            {
                characterMove.Hurt(damage);
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
