using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPoolable : MonoBehaviour, EnemyPool.IPoolTarget
{
    public ObjectPool<EnemyPoolable> Owner { get; set; }
    Cached<Rigidbody> cached_rb;
    Rigidbody RB => cached_rb[this];

    GameObjectPool DeathVFXPool => ((EnemyPool)Owner)?.DeathVFXPool;
    GameObjectPool HealthPickupPool => ((EnemyPool)Owner)?.HealthPickupPool;
    [SerializeField] private Vector2Int healthPickupDropRange;
    [SerializeField] private float healthDropRadius;

    void ObjectPool.IPoolTarget.OnDespawn()
    {
        if (DeathVFXPool) DeathVFXPool.Pull(transform.position, Quaternion.identity);
        if (HealthPickupPool)
        {
            var dropCount = Random.Range(healthPickupDropRange.x, healthPickupDropRange.y);
            for (int i = 0; i < dropCount; i++)
                HealthPickupPool.Pull(transform.position + Random.insideUnitCircle._x0y() * healthDropRadius, Quaternion.identity);
        }
        gameObject.SetActive(false);
        RB.velocity = default;
        RB.Sleep();
    }

    void ObjectPool.IPoolTarget.OnSpawn()
    {
        gameObject.SetActive(true);
    }
}
