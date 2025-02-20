using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private LootPool<EnemyPool> enemyPoolPool = new();
    [SerializeField] private EnemyPoolable[] enemyTemplates;
    [SerializeField] private EnemyPool poolTemplate;
    private EnemyPool[] enemyPools;
    int DesiredEnemyCount => 5;
    int EnemyCount => enemyPools.Sum(pool => pool.InCirculation);

    [SerializeField] private GameObjectPool HitVFXPool;
    [SerializeField] private GameObjectPool DeathVFXPool;
    [SerializeField] private GameObjectPool HealthPickupPool;

    void Awake()
    {
        var template = poolTemplate.Template;
        enemyPools = new EnemyPool[enemyTemplates.Length];
        for (int i = 0; i < enemyTemplates.Length; i++)
        {
            poolTemplate.Template = enemyTemplates[i];
            var pool = Instantiate(poolTemplate, transform);
            pool.HealthPickupPool = HealthPickupPool;
            pool.DeathVFXPool = DeathVFXPool;
            pool.HitVFXPool = HitVFXPool;
            enemyPools[i] = pool;
            enemyPoolPool.Push(pool);
        }
        poolTemplate.Template = template;
    }

    void FixedUpdate()
    {
        while (EnemyCount < DesiredEnemyCount)
            DoSpawn();
    }

    void DoSpawn()
    {
        var enemy = enemyPoolPool.Pull().Pull();
    }
}