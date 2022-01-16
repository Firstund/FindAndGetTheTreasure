using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchCharacter : EnemyStatus
{
    [SerializeField]
    private LayerMask whatIsHit;
    [SerializeField]
    private LayerMask whatIsGround;
    private float distance = 0f;
    public States CheckStatus(Vector2 playerPosition, bool isShootProjectile, float canLookPlayerY, float foundRange,float shootRange , float attackRange)
    {
        if(Mathf.Abs(playerPosition.y - transform.position.y) > canLookPlayerY)
        {
            return States.Searching;
        }

        distance = Vector2.Distance(transform.position, playerPosition);

        Vector2 currentPosition = transform.position;

        Vector2 position = playerPosition - currentPosition;

        Ray2D ray = new Ray2D(currentPosition, position);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, 10f, whatIsHit);

        Debug.DrawRay(currentPosition, hit.point - currentPosition, new Color(0, 1, 0));

        if (hit)
        {
            if (1 << hit.collider.gameObject.layer == LayerMask.GetMask("PLAYER"))
            {
                if (distance <= attackRange)
                {
                    return States.Attack;
                }
                else if(distance <= shootRange && isShootProjectile)
                {
                    return States.Shoot;
                }
                else if (distance <= foundRange)
                {
                    return States.Found;
                }
                else
                {
                    return States.Searching;
                }
            }
            else
            {
                return States.Searching;
            }
        }
        else
        {
            return States.Searching;
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
}
