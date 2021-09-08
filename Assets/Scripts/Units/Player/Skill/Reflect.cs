using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reflect : MonoBehaviour
{
    private GameManager gameManager = null;

    private bool isAttack = false;
    public bool canShoot { private get; set; }
    public bool canSettingAngle { private get; set; }
    [SerializeField]
    private float upDownSpeed = 1f;

    [SerializeField]
    private GameObject projectile = null;
    [SerializeField]
    private LineRenderer projectileShootLine = null;
    private PlayerInput playerInput = null;

    [SerializeField]
    private Transform shootTrm = null;
    private Vector2 shootAngle = Vector2.zero;
    void Start() /////////////////가끔 게임 멈추는 버그
    {
        gameManager = GameManager.Instance;

        gameManager.StopSlowTimeByLerp += () =>
        {
            isAttack = false;
            canShoot = false;

            for (int i = 0; i < 2; i++)
            {
                projectileShootLine.SetPosition(i, Vector2.zero);
            }
        };

        playerInput = GetComponent<PlayerInput>();

        canSettingAngle = true;
        canShoot = true;
    }

    void Update()
    {
        if (canShoot && playerInput.isAttack)
        {
            isAttack = true;
        }
    }
    void FixedUpdate()
    {
        if (canSettingAngle)
        {
            SettingAngle();
        }
        if (canShoot)
        {
            gameManager.SetSlowTimeByLerp(15f);
            Shoot();
        }
    }
    public void Shoot()
    {
        if (isAttack)
        {
            Instantiate(projectile, shootTrm.position, Quaternion.identity).GetComponent<EnemyProjectile>().SpawnSet(false, 10, 1, shootAngle - (Vector2)shootTrm.position);

            gameManager.StopSlowTimeByLerp();
        }
    }
    public void SettingAngle()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            shootAngle.y += upDownSpeed;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            shootAngle.y -= upDownSpeed;
        }

        projectileShootLine.SetPosition(0, shootTrm.position);
        projectileShootLine.SetPosition(1, shootAngle - (Vector2)shootTrm.position);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Ray2D ray = new Ray2D();
        ray.origin = (Vector2)shootTrm.position;
        ray.direction = (shootAngle - (Vector2)shootTrm.position);

        Gizmos.DrawRay(ray.origin, ray.direction);
    }
}
