using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : Projectile_Base, IProjectile
{
    [SerializeField]
    private GameObject attackSoundBox = null;
    private bool soundBoxSpawned = false;
    void Update()
    {
        Move();
        Despawn();
        GetDamage();
    }
    public void SpawnSet(float shootR, float dm, Vector2 angle)
    {
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

        foreach (var item in stageManager.Enemys)
        {
            if (item.activeSelf)
            {
                distance = Vector2.Distance(item.transform.position, transform.position);

                if (distance <= hitRange)
                {
                    EnemyMove enemyMove = item.GetComponent<EnemyMove>();

                    enemyMove.Hurt(damage);

                    if (!soundBoxSpawned)
                    {
                        stageManager.SpawnSoundBox(attackSoundBox);

                        soundBoxSpawned = true;
                    }

                    // isDestroy = true;
                    // stageManager.DespawnProjectile(gameObject);
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

                stageManager.DespawnProjectile(gameObject);
            }
        }
    }
}
