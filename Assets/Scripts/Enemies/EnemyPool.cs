using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : ObjectPool<EnemyPoolable>
{
    public EnemySpawner Spawner { get; set; }
    public GameObjectPool DeathVFXPool => Spawner.DeathVFXPool;
    public GameObjectPool HitVFXPool => Spawner.HitVFXPool;
    public GameObjectPool HealthPickupPool => Spawner.HealthPickupPool;
    public EnemyPoolable Pull(Vector3 position, Quaternion rotation)
    {
        var instance = this.Pull();
        instance.transform.SetPositionAndRotation(position, rotation);
        return instance;
    }

    public override void OnCreate(EnemyPoolable instance)
    {
        instance.gameObject.SetActive(false);
    }
}
