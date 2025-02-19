using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : ObjectPool<EnemyPoolable>
{
    [field: SerializeField] public GameObjectPool DeathVFXPool { get; private set; }
    public void Pull(Vector3 position, Quaternion rotation)
    {
        var instance = this.Pull();
        instance.transform.SetPositionAndRotation(position, rotation);
    }

    public override void OnCreate(EnemyPoolable instance)
    {
        instance.gameObject.SetActive(false);
    }
}
