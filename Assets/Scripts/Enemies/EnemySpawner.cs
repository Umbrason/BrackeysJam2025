using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private LootPool<EnemyPool> enemyPoolPool = new();
    [SerializeField] private EnemyPoolable[] enemyTemplates;
    [SerializeField] private EnemyPool poolTemplate;
    private EnemyPool[] enemyPools;
    public int DesiredEnemyCount {get; set;}
    public int EnemyCount => enemyPools.Sum(pool => pool.InCirculation);

    private float enemyIncreaseTimer;
    [SerializeField] private int enemyIncreaseTick;
    [SerializeField] private int enemyAmountGrowth;
    [SerializeField] private GameObjectPool HitVFXPool;
    [SerializeField] private GameObjectPool DeathVFXPool;
    [SerializeField] private GameObjectPool HealthPickupPool;
    [SerializeField] private float randomSpawnRange = 200;
    [SerializeField] private float worldBorder;
    [SerializeField] private float playerVisionRadius = 15;

    [SerializeField] private Transform player;
    
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
        enemyIncreaseTimer += Time.deltaTime;
        while (enemyIncreaseTimer >= enemyIncreaseTick)
        { 
             enemyIncreaseTimer -= enemyIncreaseTick;
             DesiredEnemyCount += enemyAmountGrowth;
        }

        while (EnemyCount < DesiredEnemyCount)
            DoSpawn();
    }

    public void DoSpawn()
    {
        var enemy = enemyPoolPool.Pull().Pull();
        var radius = enemy.GetComponentInChildren<SphereCollider>().radius;
        var enemyPosition = generateSpawnPosition(radius);
        enemy.transform.SetLocalPositionAndRotation(enemyPosition, Quaternion.identity);
    }


    const int maxAttempts = 10;
    Collider[] _discard = new Collider[0];
    Vector3 generateSpawnPosition(float radius)
    {        
        var spawnLocation = Vector3.zero;
        for(int i = 0; i < maxAttempts; i++)
        {
            spawnLocation = Random.insideUnitCircle._x0y() * randomSpawnRange;
            var distanceToWorld = (spawnLocation - Vector3.zero).sqrMagnitude;
            var distanceToPlayer = (spawnLocation - player.position).sqrMagnitude;
            if(distanceToWorld > worldBorder * worldBorder || distanceToPlayer < playerVisionRadius * playerVisionRadius)
                continue;            
            if (Physics.OverlapSphereNonAlloc(spawnLocation, radius, _discard, LayerMask.GetMask("Obstacles")) > 0)
                continue;
            break;
        }
        return spawnLocation;
    }
}
