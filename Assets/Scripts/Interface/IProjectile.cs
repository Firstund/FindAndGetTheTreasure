using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IProjectile
{
   void SpawnSet(float shootR, float dm, Vector2 angle);
   void Move();
   void GetDamage();
   void Despawn();

}
