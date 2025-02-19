using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPoolable : MonoBehaviour, EnemyPool.IPoolTarget
{
    public ObjectPool<EnemyPoolable> Owner { get; set; }
    Cached<Rigidbody> cached_rb;
    Rigidbody RB => cached_rb[this];

    GameObjectPool DeathVFXPool => ((EnemyPool)Owner)?.DeathVFXPool;

    void ObjectPool.IPoolTarget.OnDespawn()
    {
        if (DeathVFXPool) DeathVFXPool.Pull(transform.position, Quaternion.identity);
        gameObject.SetActive(false);
        RB.velocity = default;
        RB.Sleep();
    }

    void ObjectPool.IPoolTarget.OnSpawn()
    {
        gameObject.SetActive(true);
    }
}
