using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagableObstacle : MonoBehaviour
{
    private GameManager gameManager = null;
    private CharacterMove player = null;
    [SerializeField]
    private LayerMask whatIsPlayer;


    [Header("왼쪽 오른쪽 아래 위 / 체크한 방향의 위치에서 플레이어가 부딪혔을 때만 데미지 체크")]
    [SerializeField]
    private bool[] direction = new bool[4];
    private bool[] playerDirection = new bool[4];
    [SerializeField]
    private float damage = 1f;
    private bool playerDamage = false;

    void Start()
    {
        gameManager = GameManager.Instance;

        player = gameManager.player;
    }

    void FixedUpdate()
    {
        // 테스트용
        Debug.DrawLine(transform.position, (Vector2)transform.position + (Vector2.left + Vector2.up) * 10f, Color.red, 10f);
        Debug.DrawLine(transform.position, (Vector2)transform.position + (Vector2.left + Vector2.down) * 10f, Color.red, 10f);
        Debug.DrawLine(transform.position, (Vector2)transform.position + (Vector2.right + Vector2.up) * 10f, Color.red, 10f);
        Debug.DrawLine(transform.position, (Vector2)transform.position + (Vector2.right + Vector2.down) * 10f, Color.red, 10f);

        CheckDirection();

        if (playerDamage)
        {
            if ((playerDirection[0] && direction[0]) ||
                (playerDirection[1] && direction[1]) ||
                (playerDirection[2] && direction[2]) ||
                (playerDirection[3] && direction[3])
            )
            {
                player.Hurt(damage);
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (1 << col.gameObject.layer == LayerMask.GetMask("PLAYER")) // 플레이어 캐릭터인지 체크
        {
            playerDamage = true;
        }
    }
    private void OnCollisionExit2D(Collision2D col)
    {
        if (1 << col.gameObject.layer == LayerMask.GetMask("PLAYER"))
        {
            playerDamage = false;
        }
    }
    private void CheckDirection()
    {
        float playerX = player.transform.position.x;
        float playerY = player.transform.position.y;

        if (playerX <= GetPlayerXOnLineGraph(1f, playerY) && playerX <= GetPlayerXOnLineGraph(-1f, playerY))
        {
            playerDirection[0] = true;
            playerDirection[1] = false;
        }
        else if (playerX > GetPlayerXOnLineGraph(1f, playerY) && playerX > GetPlayerXOnLineGraph(-1f, playerY))
        {
            playerDirection[1] = true;
            playerDirection[0] = false;
        }
        else
        {
            playerDirection[1] = false;
            playerDirection[0] = false;
        }

        if (playerY <= GetPlayerYOnLineGraph(1f, playerX) && playerY <= GetPlayerYOnLineGraph(-1f, playerX))
        {
            playerDirection[2] = true;
            playerDirection[3] = false;
        }
        else if (playerY > GetPlayerYOnLineGraph(1f, playerX) && playerY > GetPlayerYOnLineGraph(-1f, playerX))
        {
            playerDirection[3] = true;
            playerDirection[2] = false;
        } 
        else
        {
            playerDirection[3] = false;
            playerDirection[2] = false;
        }
    }
    private float GetPlayerYOnLineGraph(float a, float playerX)
    {
        float returnY = a * playerX + transform.position.y - (a * transform.position.x);
        Debug.DrawLine(transform.position, new Vector2(playerX, returnY), Color.blue);
        return returnY;
    }
    private float GetPlayerXOnLineGraph(float a, float playerY)
    {
        float returnX = ((playerY - transform.position.y + (a * transform.position.x)) / a);

        Debug.DrawLine(transform.position, new Vector2(returnX, playerY), Color.green);

        return returnX;
    }
}
