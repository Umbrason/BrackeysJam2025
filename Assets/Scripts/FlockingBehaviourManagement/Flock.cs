using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace FlockingBehaviourManagement
{
    public class Flock : MonoBehaviour
    {
        [SerializeField]private float agentDensity = 0.08f;
        [SerializeField]private FlockBehaviour behaviour;
        [Range(1f, 100f)]
        private float driveFactor = 10f;
        [Range(1f, 100f)]
        private float maxSpeed = 5f;
        [Range(1f, 10f)]
        private float neighborRadius = 1.5f;
        [Range(0f, 1f)]
        private float avoidanceRadiusMultiplier = 0.5f;
        
        private float squareMaxSpeed;
        private float squareNeighborRadius;
        private float squareAvoidanceRadius;
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

            squareMaxSpeed = data.maxSpeed * data.maxSpeed;
            squareNeighborRadius = data.neighborRadius * data.neighborRadius;
            squareAvoidanceRadius = squareNeighborRadius * data.avoidanceRadiusMultiplier * data.avoidanceRadiusMultiplier;

            foreach(var agent in agents)
            {
                agent.transform.position = Random.insideUnitCircle * agents.Count * agentDensity;
                agent.Initialize(this);
            }
        }

        private List<Transform> GetNearbyObjects(FlockAgent agent)
        {
            var nearbyObjects = new List<Transform>();
            var agentPosition = agent.transform.position;
            for(int i = 0; i < FlockAgent.AllFlockAgents.Count; i++)
            {
                FlockAgent current = FlockAgent.AllFlockAgents[i];
                if(current == null || current == agent || !current.gameObject.activeInHierarchy)
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
                Vector2 move = behaviour.CalculateMove(current, context, this);

                move *= driveFactor;
                if(move.sqrMagnitude > squareMaxSpeed)
                {
                    move = move.normalized * maxSpeed;
                }
                current.Move(move);
            }    
        }
    }

}
