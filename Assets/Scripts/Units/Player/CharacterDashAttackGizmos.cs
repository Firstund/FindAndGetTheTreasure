using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDashAttackGizmos : MonoBehaviour
{
    [SerializeField]
    private float attackRangeX = 10f;
    [SerializeField]
    private float attackRangeY = 10f;
    [SerializeField]
    private Color attack_color;
    [SerializeField]
    private float offsetX = 2.5f;

    private void OnDrawGizmos()
    {
        Gizmos.color = attack_color;

        Vector3 size;
        size.x = attackRangeX;
        size.y = attackRangeY;
        size.z = 1f;

        Vector3 currentPosition = transform.position;
        currentPosition.x += offsetX;

        Gizmos.DrawWireCube(currentPosition, size);
    }
}
