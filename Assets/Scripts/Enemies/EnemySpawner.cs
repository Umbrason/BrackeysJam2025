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

    private float enemyIncreaseTimer;
    [SerializeField] private int enemyIncreaseTick;
    [SerializeField] private int enemyAmountGrowth;
    private int enemyDesiredCountIncrease;
    void Awake()
    {
        foreach (var enemyPool in enemyPools)
            enemyPoolPool.Push(enemyPool);
    }

    void FixedUpdate()
    { 
        enemyIncreaseTimer += Time.deltaTime;
        while (enemyIncreaseTimer >= enemyIncreaseTick)
        { 
             enemyIncreaseTimer -=enemyIncreaseTick;
             enemyDesiredCountIncrease+= enemyAmountGrowth;
        }

        while (EnemyCount < DesiredEnemyCount + enemyDesiredCountIncrease)
            DoSpawn();
    }

    void DoSpawn()
    {
        var enemy = enemyPoolPool.Pull().Pull();
        generateSpawnPosition();
    }
    
    Vector3 generateSpawnPosition()
    {
        return new Vector3(0,0,0);

    }

}