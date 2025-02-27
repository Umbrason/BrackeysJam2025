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
    public int DesiredEnemyCount => CalcEnemyCount();
    public int EnemyCount => enemyPools.Sum(pool => pool.InCirculation);

    public GameObjectPool HitVFXPool;
    public GameObjectPool DeathVFXPool;
    public GameObjectPool HealthPickupPool;
    [SerializeField] private float randomSpawnRange = 200;
    [SerializeField] private float worldBorder;
    [SerializeField] private float playerVisionRadius = 15;

    [SerializeField] private Transform player;

    private float startTime;
    void Awake()
    {
        var template = poolTemplate.Template;
        enemyPools = new EnemyPool[enemyTemplates.Length];
        for (int i = 0; i < enemyTemplates.Length; i++)
        {
            poolTemplate.Template = enemyTemplates[i];
            var pool = Instantiate(poolTemplate, transform);
            pool.Spawner = this;
            enemyPools[i] = pool;
        }
        AddNextEnemyType();
        poolTemplate.Template = template;
        startTime = Time.time;
    }

    int CalcEnemyCount()
    {
        var playtime = Time.time - startTime;
        playtime = Mathf.Min(12000, playtime + 15 * TransientScoring.UpgradesCollected);
        var a = Mathf.Pow(playtime, 1.4f) * 0.022f / 3f;
        var b = Mathf.Pow(playtime, 1.5f) * 0.008f / 3f;
        return Mathf.FloorToInt((a - b) * .4f + 2);
    }

    public void AddNextEnemyType()
    {
        if (enemyPoolPool.Size >= enemyPools.Length)
            return;
        enemyPoolPool.Push(enemyPools[enemyPoolPool.Size]);
    }

    void FixedUpdate()
    {
        var missingEnemies = DesiredEnemyCount - EnemyCount;
        for (int i = 0; i < missingEnemies; i++)
            DoSpawn();
    }

    public void DoSpawn()
    {
        var enemy = enemyPoolPool.Pull().Pull();
        var radius = enemy.GetComponentInChildren<SphereCollider>().radius;
        var enemyPosition = GenerateSpawnPosition(radius);
        enemy.transform.SetLocalPositionAndRotation(enemyPosition, Quaternion.identity);
    }


    private const int maxAttempts = 20;
    private static readonly Collider[] _discard = new Collider[1];
    public Vector3 GenerateSpawnPosition(float radius)
    {
        var spawnLocation = Vector3.zero;
        for (int i = 0; i < maxAttempts; i++)
        {
            var angle = Random.value * Mathf.PI * 2;
            var mag = Random.value * (randomSpawnRange - playerVisionRadius) + playerVisionRadius;
            var dir = new Vector3(Mathf.Sin(angle), 0, Mathf.Cos(angle));
            spawnLocation = player.position + dir * mag;
            if (spawnLocation.sqrMagnitude > worldBorder * worldBorder) continue;
            if (Physics.OverlapSphereNonAlloc(spawnLocation, radius, _discard, LayerMask.GetMask("Obstacles")) > 0) continue;
            break;
        }
        return spawnLocation;
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        UnityEditor.Handles.color = Color.green;
        UnityEditor.Handles.DrawWireDisc(transform.position + Vector3.up, Vector3.up, worldBorder);
        UnityEditor.Handles.color = Color.red;
        UnityEditor.Handles.DrawWireDisc(player.position + Vector3.up, Vector3.up, randomSpawnRange);
        UnityEditor.Handles.color = Color.blue;
        UnityEditor.Handles.DrawWireDisc(player.position + Vector3.up, Vector3.up, playerVisionRadius);
    }
#endif
}
