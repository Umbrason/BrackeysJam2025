using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using FlockingBehaviourManagement;

namespace EnemyManagement
{
    public class EnemyGenerator : MonoBehaviour
    {
        [SerializeField] private Flock flockPrefab;
        [SerializeField] private FlockAgent flockAgentPrefab;
        [Min(10)]
        [SerializeField] private int maxFlockAgentCount = 10;
        [SerializeField] private FlockData flockData;
        [SerializeField] private Transform target;

        private ObjectPool<FlockAgent> flockAgentPool;

        private void CreateFlock()
        {
            if (flockData == null)
            {
                return;
            }

            Flock flock = Instantiate(flockPrefab, transform.position, Quaternion.identity);

            int i = 1;
            while (i <= maxFlockAgentCount)
            {
                FlockAgent agent = flockAgentPool.Get();
                agent.transform.forward = Vector3.forward;
                agent.transform.parent = flock.transform;
                flock.Agents.Add(agent);
                i++;
            }

            flock.Initialize(flockData, target);
            
        }

        private FlockAgent CreateFlockAgent()
        {
            var agent = Instantiate(flockAgentPrefab, transform.position, Quaternion.identity);
            agent.gameObject.SetActive(false);
            return agent;
        }

        private void OnGetFlockAgent(FlockAgent agent)
        {
            agent.gameObject.SetActive(true);
        }

        private void OnReleaseFlockAgent(FlockAgent agent)
        {
            agent.gameObject.SetActive(false);
        }

        private void OnDestroyFlockAgent(FlockAgent agent)
        {
            Destroy(agent.gameObject);
        }

        // Start is called before the first frame update
        void Start()
        {
            if(flockAgentPool == null)
            {
                flockAgentPool = new ObjectPool<FlockAgent>(CreateFlockAgent,
                                                            OnGetFlockAgent,
                                                            OnReleaseFlockAgent,
                                                            OnDestroyFlockAgent,
                                                            true,
                                                            maxFlockAgentCount,
                                                            maxFlockAgentCount);
            }

            CreateFlock();
        }
    }

}
