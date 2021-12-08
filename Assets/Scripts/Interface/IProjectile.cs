using UnityEngine;

public interface IProjectile
{
   void SpawnSet(float shootR, float dm, Vector2 angle);
   void SpawnSet(float shootR, float dm, Vector2 dir, float a);
   void Move();
   void GetDamage();
   void Despawn();

}
