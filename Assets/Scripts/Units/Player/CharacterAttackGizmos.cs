using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAttackGizmos : MonoBehaviour
{
    [SerializeField]
    private float attackRange = 10f;
    [SerializeField]
    private Color attack_color;

    private void OnDrawGizmos()
    {
        Gizmos.color = attack_color;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
