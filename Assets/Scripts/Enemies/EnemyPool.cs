using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : ObjectPool<EnemyPoolable>
{
    public GameObjectPool DeathVFXPool { get; set; }
    public GameObjectPool HitVFXPool { get; set; }
    public GameObjectPool HealthPickupPool { get; set; }
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
