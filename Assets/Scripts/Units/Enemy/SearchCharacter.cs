using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchCharacter : EnemyStatus
{
    private float distance = 0f;
    public Status CheckStatus(Vector2 playerPosition, float foundRange, float attackRange)
    {
        distance = Vector2.Distance(transform.position, playerPosition);

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
