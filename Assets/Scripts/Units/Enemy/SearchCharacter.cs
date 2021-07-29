using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchCharacter : EnemyStatus
{
    [SerializeField]
    private LayerMask whatIsHit;
    [SerializeField]
    private LayerMask whatIsPlayer;
    private float distance = 0f;
    public Status CheckStatus(Vector2 playerPosition, float foundRange, float attackRange)
    {
        distance = Vector2.Distance(transform.position, playerPosition);

        Vector2 currentPosition = transform.position;

        Vector2 position = playerPosition - currentPosition;

        Ray2D ray = new Ray2D(currentPosition, position);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, 10f, whatIsHit);

        Debug.DrawRay(currentPosition, hit.point - currentPosition, new Color(0, 1, 0));

        if (hit)
        {
            if (hit.collider.gameObject.layer == GetLayer(LayerMask.GetMask("PLAYER")))
            {
                if (distance <= attackRange)
                {
                    return Status.Attack;
                }
                else if (distance <= foundRange)
                {
                    return Status.Found;
                }
                else
                {
                    return Status.Searching;
                }
            }
            else
            {
                return Status.Searching;
            }
        }
        else
        {
            return Status.Searching;
        }
    }
    public bool CheckFlip(Vector2 targetPosition)
    {
        float a = targetPosition.x - transform.position.x;

        if (a < 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private int GetLayer(LayerMask a)
    {
        int b = a;
        int result = 0;

        while (true)
        {
            if (b >= 2)
            {
                result++;
                b = b / 2;
            }
            else
            {
                break;
            }
        }

        return result;
    }
}
