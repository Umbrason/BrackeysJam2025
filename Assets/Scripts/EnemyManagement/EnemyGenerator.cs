using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using AttractorManagement;

namespace EnemyManagement
{
    public class EnemyGenerator : MonoBehaviour
    {
        [SerializeField] private AttractorAgent attractorAgentPrefab;
        [Min(0.01f)]
        [SerializeField] private float agentDensity = 0.01f;
        [Min(1)]
        [SerializeField] private int maxEnemyCount = 10;
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private Transform target;
        

        private ObjectPool<AttractorAgent> attractorAgentPool;

        private void SpawnEnemies(int enemyCount)
        {
            int i = 1;
            while(i <= enemyCount)
            {

                Vector2 randomDirection2D = Random.insideUnitCircle;
                Vector3 spawnPosition = spawnPoint.position +
                                        new Vector3(randomDirection2D.x, 0.0f, randomDirection2D.y) *
                                        enemyCount *
                                        agentDensity;

                var agent = attractorAgentPool.Get();
                agent.transform.position = spawnPosition;
                agent.Target = target;
                i++;
            }
        }

        private AttractorAgent CreateAttractorAgent()
        {
            var agent = Instantiate(attractorAgentPrefab, transform.position, Quaternion.identity);
            agent.gameObject.SetActive(false);
            return agent;
        }

        private void OnGetAttractorAgent(AttractorAgent agent)
        {
            agent.gameObject.SetActive(true);
        }

        private void OnReleaseAttractorAgent(AttractorAgent agent)
        {
            agent.gameObject.SetActive(false);
        }

        private void OnDestroyAttractorAgent(AttractorAgent agent)
        {
            Destroy(agent.gameObject);
        }

        // Start is called before the first frame update
        void Start()
        {
            if(attractorAgentPool == null)
            {
                attractorAgentPool = new ObjectPool<AttractorAgent>(CreateAttractorAgent,
                                                                    OnGetAttractorAgent,
                                                                    OnReleaseAttractorAgent,
                                                                    OnDestroyAttractorAgent,
                                                                    true,
                                                                    maxEnemyCount,
                                                                    maxEnemyCount);
            }

            SpawnEnemies(maxEnemyCount);
        }
    }

}
