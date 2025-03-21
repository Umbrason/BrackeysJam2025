using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeSpawner : MonoBehaviour
{
    private LootPool<Transform> SpawnPositions = new();
    void Awake()
    {
        for (int i = 0; i < transform.childCount; i++)
            SpawnPositions.Push(transform.GetChild(i));
        StartCoroutine(UpgradeGameLoop());
    }

    [SerializeField] private UpgradePickup pickupTemplate;
    [SerializeField] private EnemySpawner enemySpawner;
    [SerializeField] private float minDistance = 25f;
    [SerializeField] private Transform player;

    [SerializeField] private AudioClipGroup ExtraEnemiesVoiceLines;


    IEnumerator UpgradeGameLoop()
    {
        Instantiate(pickupTemplate, SpawnPositions.Pull());
        while (true)
        {
            yield return new WaitUntil(() => UpgradePickup.Instance == null);


            yield return new WaitForSeconds(5);
            enemySpawner.AddNextEnemyType();
            for (int i = 0; i < enemySpawner.DesiredEnemyCount * 3; i++)
                enemySpawner.DoSpawn(); //Spawn four times as many enemies as "desired" by difficulty
            VoicelinePlayer.Play(ExtraEnemiesVoiceLines, true);

            yield return new WaitUntil(() => enemySpawner.EnemyCount <= enemySpawner.DesiredEnemyCount);

            Transform spawnPoint = null;
            for (int i = 0; i < SpawnPositions.Size; i++)
            {
                spawnPoint = SpawnPositions.Pull();
                if ((spawnPoint.position - player.position).sqrMagnitude > minDistance * minDistance)
                    break;
            }
            Instantiate(pickupTemplate, spawnPoint);
        }
    }
}
