using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieOnDeath : MonoBehaviour
{
    Cached<EnemyPoolable> cached_EnemyPoolable;
    EnemyPoolable EnemyPoolable => cached_EnemyPoolable[this];

    Cached<HealthPool> cached_HealthPool;
    HealthPool HealthPool => cached_HealthPool[this];

    void Start()
    {
        HealthPool.OnDepleted += Die;
    }

    void Die()
    {
        EnemyPoolable.Owner.Return(EnemyPoolable);
    }
}
