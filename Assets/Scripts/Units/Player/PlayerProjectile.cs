using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : Projectile_Base, IProjectile
{
    [SerializeField]
    private GameObject attackSoundBox = null;
    private GameObject targetEnemy = null;

    private bool soundBoxSpawned = false;
    private List<GameObject> Enemys = new List<GameObject>();

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

        Enemys.Clear();
        soundBoxSpawned = false;

        stageManager.Enemys.ForEach(item => Enemys.Add(item));
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
        if (other.gameObject.CompareTag("Enemy"))
        {
            targetEnemy = other.gameObject;
            GetDamage();
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

            Enemys.Remove(targetEnemy);
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
