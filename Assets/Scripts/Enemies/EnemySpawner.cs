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

    static GameObject player;
    
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
        player = GameObject.Find("Player");


    }

    void FixedUpdate()
    { 
        enemyIncreaseTimer += Time.deltaTime;
        while (enemyIncreaseTimer >= enemyIncreaseTick)
        { 
             enemyIncreaseTimer -= enemyIncreaseTick;
             DesiredEnemyCount += enemyAmountGrowth;
        }

        while (EnemyCount < DesiredEnemyCount + enemyDesiredCountIncrease)
            DoSpawn();
    }

    public void DoSpawn()
    {
        var enemy = enemyPoolPool.Pull().Pull();
        var radius = enemy.GetComponentInChildren<SphereCollider>().radius;
        Vector3 enemyPosition = generateSpawnPosition();
        enemy.transform.SetLocalPositionAndRotation(enemyPosition, quaternion.identity);
    }


    const int maxAttempts = 10;
    Collider[] _discard = new Collider[0];
    Vector3 generateSpawnPosition(float radius)
    {        
        Vector3 spawnLocation;
        for(int i = 0; i < maxAttempts; i++)
        {
            spawnLocation = UnityEngine.Random.InsideUnitSphere * randomSpawnRange;
            distanceToWorld = Vector3.Distance(spawnLocation, Vector3.zero);
            distanceToPlayer = Vector3.Distance(spawnLocation,player.transform.position);       
            if(distanceToWorld > worldBorder || distanceToPlayer < playerVisionRadius)
                continue;            
            if (Physics.OverlapSphereNonAlloc(spawnLocation, radius, _discard, LayerMask.GetMask("Obstacles")) > 0)
                continue;
            break;
        }
        return spawnLocation;
    }
}
