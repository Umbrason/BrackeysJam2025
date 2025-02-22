using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPoolable : MonoBehaviour, EnemyPool.IPoolTarget
{
    public ObjectPool<EnemyPoolable> Owner { get; set; }
    Cached<Rigidbody> cached_rb;
    Rigidbody RB => cached_rb[this];

    GameObjectPool HitVFXPool => ((EnemyPool)Owner)?.HitVFXPool;
    GameObjectPool DeathVFXPool => ((EnemyPool)Owner)?.DeathVFXPool;
    GameObjectPool HealthPickupPool => ((EnemyPool)Owner)?.HealthPickupPool;
    [SerializeField] private Vector2Int healthPickupDropRange;
    [SerializeField] private float healthDropRadius;

    Cached<Hitbox> cached_Hitbox = new(Cached<Hitbox>.GetOption.Children);
    Hitbox Hitbox => cached_Hitbox[this];

    void Awake()
    {
        Hitbox.OnDamageEventRegistered += (evt) =>
        {
            var pos = transform.position;
            var srcPos = evt.Source?.transform?.position ?? transform.position + Vector3.up;
            var delta = pos - srcPos;
            HitVFXPool.Pull(transform.position, Quaternion.LookRotation(delta));
        };
    }

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
