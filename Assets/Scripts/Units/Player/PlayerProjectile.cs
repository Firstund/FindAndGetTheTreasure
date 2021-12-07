using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : Projectile_Base, IProjectile
{
    [SerializeField]
    private GameObject attackSoundBox = null;
    private GameObject targetEnemy = null;

    private bool soundBoxSpawned = false;
    private List<GameObject> TargetObjs = new List<GameObject>();

    void Update()
    {
        Move();
        Despawn();
    }
    private void OnEnable()
    {
        if (stageManager == null)
        {
            stageManager = StageManager.Instance;
        }

        TargetObjs.Clear();
        soundBoxSpawned = false;

        stageManager.Enemys.ForEach(item => TargetObjs.Add(item));
    }
    public void SpawnSet(float shootR, float dm, Vector2 dir)
    {
        firstPosition = transform.position;
        isDestroy = false;
        shootRange = shootR;
        damage = dm;
        shootDir = dir;
    }
    public void SpawnSet(float shootR, float dm, Vector2 dir, float a)
    {
        firstPosition = transform.position;
        isDestroy = false;
        shootRange = shootR;
        damage = dm;
        shootDir = dir;
        spriteRenderer.color = new Vector4(1f, 1f, 1f, a);
    }
    public void Move()
    {
        transform.Translate(shootDir * speed * Time.fixedDeltaTime);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject != null)
        {
            targetEnemy = other.gameObject;
        }

        if (other.gameObject.CompareTag("Enemy"))
        {
            GetDamage();
        }
        else if (other.gameObject.CompareTag("Boss"))
        {
            GetBossDamage();
        }
    }
    public void GetDamage()
    {
        if (targetEnemy.activeSelf)
        {
            EnemyMove enemyMove = targetEnemy.GetComponent<EnemyMove>();

            enemyMove.Hurt(damage);

            if (!soundBoxSpawned)
            {
                stageManager.SpawnSoundBox(attackSoundBox);

                soundBoxSpawned = true;
            }

            TargetObjs.Remove(targetEnemy);
        }
    }
    public void GetBossDamage()
    {
        if (targetEnemy.activeSelf)
        {
            BossStat bossStat = targetEnemy.GetComponent<BossStat>();

            if (!bossStat.IsNothurtMode)
            {
                bossStat.Hurt(damage);

                if (!soundBoxSpawned)
                {
                    stageManager.SpawnSoundBox(attackSoundBox);

                    soundBoxSpawned = true;
                }

                TargetObjs.Remove(targetEnemy);
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
