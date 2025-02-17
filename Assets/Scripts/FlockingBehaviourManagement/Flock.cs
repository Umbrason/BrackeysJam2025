using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace FlockingBehaviourManagement
{
    public class Flock : MonoBehaviour
    {
        [SerializeField] private float agentDensity = 0.08f;
        [SerializeField] private FlockBehaviour behaviour;
        [Range(1f, 100f)]
        [SerializeField] private float driveFactor = 10f;
        [Range(1f, 100f)]
        [SerializeField] private float maxSpeed = 5f;
        [Range(1f, 10f)]
        [SerializeField] private float neighborRadius = 1.5f;
        [Range(0f, 1f)]
        [SerializeField] private float avoidanceRadiusMultiplier = 0.5f;
        [SerializeField] private bool toggleGizmos = false;

        private float avoidanceRadius;
        private float squareMaxSpeed;
        private float squareNeighborRadius;
        private float squareAvoidanceRadius;
        private Collider[] colliders = null;
        private List<FlockAgent> agents;
        public float SquareAvoidanceRadius { get { return squareAvoidanceRadius; } }

        public List<FlockAgent> Agents { get => agents; }

        public void Initialize(FlockData data)
        {
            this.agentDensity = data.agentDensity;
            this.behaviour = data.behaviour;
            this.driveFactor = data.driveFactor;
            this.maxSpeed = data.maxSpeed;
            this.neighborRadius = data.neighborRadius;
            this.avoidanceRadiusMultiplier = data.avoidanceRadiusMultiplier;

            avoidanceRadius = data.neighborRadius * data.avoidanceRadiusMultiplier;

            squareMaxSpeed = data.maxSpeed * data.maxSpeed;
            squareNeighborRadius = data.neighborRadius * data.neighborRadius;
            squareAvoidanceRadius = squareNeighborRadius * data.avoidanceRadiusMultiplier * data.avoidanceRadiusMultiplier;

            foreach(var agent in agents)
            {
                Vector2 randomDirection2D = Random.insideUnitCircle;
                agent.transform.position = new Vector3(randomDirection2D.x, 0.0f, randomDirection2D.y) * 
                                               agents.Count * 
                                               agentDensity;
                agent.Initialize(this);
            }
        }

        private List<Transform> GetNearbyObjects(FlockAgent agent)
        {
            var nearbyObjects = new List<Transform>();
            var agentPosition = agent.transform.position;

            int layerMask = 1 << agent.gameObject.layer;
            int count = Physics.OverlapSphereNonAlloc(agentPosition, avoidanceRadius, colliders, layerMask);

            for(int i = 0; i < count; i++)
            {
                Collider current = colliders[i];

                if(current == null || current == agent.AgentCollider || !current.gameObject.activeInHierarchy)
                {
                    continue;
                }
                float squareDistance = Vector3.SqrMagnitude(current.transform.position - agentPosition);
                if(squareDistance <= squareAvoidanceRadius)
                {
                    nearbyObjects.Add(current.transform);
                }
            }

            return nearbyObjects;
        }

        private void Awake() 
        {
            agents = new List<FlockAgent>();
            colliders = new Collider[15];
        }

        private void FixedUpdate() 
        {
            for(int i = 0; i < agents.Count; i++)
            {
                var current = agents[i];
                if(current == null || !current.gameObject.activeInHierarchy)
                {
                    continue;
                }

                var context = GetNearbyObjects(current);
                Vector3 move = behaviour.CalculateMove(current, context, this);

                move *= driveFactor;
                if(move.sqrMagnitude > squareMaxSpeed)
                {
                    move = move.normalized * maxSpeed;
                }
                current.Move(move);
            }    
        }

        private void OnValidate()
        {
            avoidanceRadius = neighborRadius * avoidanceRadiusMultiplier;
            squareMaxSpeed = maxSpeed * maxSpeed;
            squareNeighborRadius = neighborRadius * neighborRadius;
            squareAvoidanceRadius = squareNeighborRadius * avoidanceRadiusMultiplier * avoidanceRadiusMultiplier;
        }

        private void OnDrawGizmosSelected()
        {
            if(!toggleGizmos)
            {
                return;
            }
            Gizmos.color = Color.yellow;

            for(int i = 0; i < agents.Count; i++)
            {
                FlockAgent current = agents[i];
                if (current == null || !current.gameObject.activeInHierarchy)
                {
                    continue;
                }
                Gizmos.DrawWireSphere(current.transform.position, avoidanceRadius);
            }
        }
    }

}
