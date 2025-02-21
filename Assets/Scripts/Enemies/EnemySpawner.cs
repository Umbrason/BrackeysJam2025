using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;



public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private LootPool<EnemyPool> enemyPoolPool = new();
    [SerializeField] private EnemyPoolable[] enemyTemplates;
    [SerializeField] private EnemyPool poolTemplate;
    private EnemyPool[] enemyPools;
    public int DesiredEnemyCount => 5;
    public int EnemyCount => enemyPools.Sum(pool => pool.InCirculation);

    private float enemyIncreaseTimer;
    [SerializeField] private int enemyIncreaseTick;
    [SerializeField] private int enemyAmountGrowth;
    private int enemyDesiredCountIncrease;
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
             enemyIncreaseTimer -=enemyIncreaseTick;
             enemyDesiredCountIncrease+= enemyAmountGrowth;
        }

        while (EnemyCount < DesiredEnemyCount + enemyDesiredCountIncrease)
            DoSpawn();
    }

    public void DoSpawn()
    {
        var enemy = enemyPoolPool.Pull().Pull( new Vector3(0,0,-10), quaternion.identity);
        var radius = enemy.GetComponentInChildren<SphereCollider>().radius;
        Vector3 enemyPosition = generateSpawnPosition();

        enemy.transform.SetLocalPositionAndRotation(enemyPosition, quaternion.identity);
        //TODO change 3 to dynamic layermask int of obstacles
        Collider[] invalidCollisionList = Physics.OverlapSphere(enemyPosition, radius, LayerMask.GetMask("Obstacles"));
        if (invalidCollisionList.Length > 0 )
        {
            enemy.Owner.Return(enemy);
            ArrayUtility.Clear<Collider>(ref invalidCollisionList);
        }
        else{
            enemy.transform.SetPositionAndRotation(enemyPosition, quaternion.identity);
            }

        

    }
    
    Vector3 generateSpawnPosition()
    {
        float locationRange = UnityEngine.Random.Range(-randomSpawnRange, randomSpawnRange);
        float locationRangeY = UnityEngine.Random.Range(-randomSpawnRange, randomSpawnRange);

        Vector3 spawnLocation = new Vector3 (locationRange, 0, locationRangeY); 
        float distanceToWorld = Vector3.Distance(spawnLocation, Vector3.zero);
        float distanceToPlayer = Vector3.Distance(spawnLocation,player.transform.position);
        while (distanceToWorld > worldBorder || distanceToPlayer < playerVisionRadius)      
        {
            locationRange = UnityEngine.Random.Range(-randomSpawnRange, randomSpawnRange);
            locationRangeY = UnityEngine.Random.Range(-randomSpawnRange, randomSpawnRange);
            spawnLocation = new Vector3 (locationRange, 0, locationRangeY);
            distanceToWorld = Vector3.Distance(spawnLocation, Vector3.zero);
            distanceToPlayer = Vector3.Distance(spawnLocation,player.transform.position);       
        }
        return spawnLocation;
        

    }

}