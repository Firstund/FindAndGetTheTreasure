using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatus : MonoBehaviour
{
    public enum States{
        Searching,
        Found,
        Attack,
        Shoot,
        Dead
    }
}
