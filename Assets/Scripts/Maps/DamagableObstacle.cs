using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagableObstacle : MonoBehaviour
{
    private GameManager gameManager = null;
    private CharacterMove player = null;
    [SerializeField]
    private LayerMask whatIsPlayer;

    [Header("왼쪽 오른쪽 위 아래 / 체크한 방향의 위치에서 플레이어가 부딪혔을 때만 데미지 체크")]
    [SerializeField]
    private bool[] direction = new bool[4];
    [SerializeField]
    private float damagableDistance = 0.5f;
    [SerializeField]
    private float damage = 1f;

    void Start()
    {
        gameManager = GameManager.Instance;

        player = gameManager.player;
    }

    void Update()
    {
        bool playerHurt = Physics2D.OverlapCircle(transform.position, damagableDistance, whatIsPlayer);

        if(playerHurt)
        {
            player.Hurt(damage);
        }
    }
}
