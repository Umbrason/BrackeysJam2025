using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private LootPool<EnemyPool> enemyPoolPool = new();
    [SerializeField] private EnemyPool[] enemyPools;
    int DesiredEnemyCount => 5;
    int EnemyCount => enemyPools.Sum(pool => pool.InCirculation);

    void Awake()
    {
        foreach (var enemyPool in enemyPools)
            enemyPoolPool.Push(enemyPool);
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