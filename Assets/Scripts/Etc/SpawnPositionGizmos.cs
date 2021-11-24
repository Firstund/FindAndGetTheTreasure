using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPositionGizmos : MonoBehaviour
{
    private void OnDrawGizmosSelected() 
    {
        Gizmos.color = Color.gray;
        Gizmos.DrawSphere(transform.position, 0.2f);    
    }
}
