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

        stageManager.Enemys.ForEach(item => Enemys.Add(item));
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
