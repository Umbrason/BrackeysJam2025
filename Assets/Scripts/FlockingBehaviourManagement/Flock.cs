using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace FlockingBehaviourManagement
{
    public class Flock : MonoBehaviour
    {
        private const int MAX_DETECT_COLLIDERS = 15;
        
        [Min(0.0f)]
        [SerializeField] private float agentDensity = 0.08f;
        
        [Range(1f, 100f)]
        [SerializeField] private float driveFactor = 10f;
        
        [Range(1f, 100f)]
        [SerializeField] private float moveSpeed = 5f;
        
        [Range(1f, 10f)]
        [SerializeField] private float neighborRadius = 1.5f;
        
        [Range(0f, 1f)]
        [SerializeField] private float avoidanceRadiusMultiplier = 0.5f;

        [SerializeField] private bool allowSteeredCohesion = false;
        
        [Range(0.0f, 1.0f)]
        public float avoidanceWeight = 1.0f;

        [Range(0.0f, 1.0f)]
        public float cohesionWeight = 1.0f;

        [Min(0.1f)]
        [SerializeField] private float agentSmoothTime = 0.5f;
        
        [SerializeField] private LayerMask obstacleLayerMask;
        
        [SerializeField] private Transform target;
        
        [SerializeField] private float minDistanceToTarget = 5.0f;

        [SerializeField] private bool toggleGizmos = false;

        private float avoidanceRadius;
        private float squareMaxSpeed;
        private float squareNeighborRadius;
        private float squareAvoidanceRadius;
        private float squareMinDistanceToTarget;
        private bool nearThePlayer = false;

        private Collider[] colliders = null;
        private List<FlockAgent> agents;
        private Vector3 currentVelocity = Vector3.zero;

        public List<FlockAgent> Agents { get => agents; }
        public Transform Target { get => target; set => target = value; }

        public void Initialize(FlockData data, Transform target)
        {
            
            this.agentDensity = data.agentDensity;
            this.moveSpeed = data.moveSpeed;
            this.neighborRadius = data.neighborRadius;
            this.avoidanceRadiusMultiplier = data.avoidanceRadiusMultiplier;
            this.avoidanceWeight = data.avoidanceWeight;
            this.cohesionWeight = data.cohesionWeight;
            this.agentSmoothTime = data.agentSmoothTime;
            this.allowSteeredCohesion = data.allowSteeredCohesion;
            this.obstacleLayerMask = data.obstacleLayerMask;
            this.target = target;
            this.minDistanceToTarget = data.minDistanceToTarget;

            avoidanceRadius = data.neighborRadius * data.avoidanceRadiusMultiplier;

            squareMaxSpeed = data.moveSpeed * data.moveSpeed;
            squareNeighborRadius = data.neighborRadius * data.neighborRadius;
            squareAvoidanceRadius = squareNeighborRadius * data.avoidanceRadiusMultiplier * data.avoidanceRadiusMultiplier;
            squareMinDistanceToTarget = data.minDistanceToTarget * data.minDistanceToTarget;

            foreach(var agent in agents)
            {
                Vector2 randomDirection2D = Random.insideUnitCircle;
                agent.transform.position = new Vector3(randomDirection2D.x, 0.0f, randomDirection2D.y) * 
                                               agents.Count * 
                                               agentDensity;
                agent.Initialize(this);
            }
        }

        private Vector3 CalculateAvoidanceVelocity(FlockAgent agent, ref List<Transform> detected)
        {
            //if no neighbors, return no adjustment
            if (detected.Count == 0)
            {
                return Vector3.zero;
            }

            Vector3 agentPosition = agent.transform.position._x0z();

            
            //add all points together and average
            Vector3 avoidanceVelocity = Vector3.zero;
            int avoidCount = 0;

            foreach (Transform item in detected)
            {
                
                Vector3 position = item.position._x0z();

                if (Vector3.SqrMagnitude(position - agentPosition) < squareAvoidanceRadius)
                {
                    avoidCount++;
                    avoidanceVelocity += agent.transform.position - position;
                }
            }

            if (avoidCount > 0)
            {
                avoidanceVelocity /= avoidCount;
            }

            return avoidanceVelocity;
        }

        private Vector3 CalculateSteeredCohesionVelocity(FlockAgent agent, ref List<Transform> detected)
        {
            //if no neighbors, return no adjustment
            if (detected.Count == 0)
            {
                return Vector3.zero;
            }

            Vector3 agentPosition = agent.transform.position._x0z();

            //add all points together and average
            Vector3 steeredCohesion = Vector3.zero;
            
            
            foreach (Transform item in detected)
            {
                Vector3 itemPosition = item.position._x0z();
                steeredCohesion += itemPosition;
            }

            steeredCohesion /= detected.Count;

            //create offset from agent position
            steeredCohesion -= agentPosition;
            steeredCohesion = Vector3.SmoothDamp(agent.transform.forward, steeredCohesion, ref currentVelocity, agentSmoothTime);
            return steeredCohesion;
        }

        private Vector3 CalculateCohesionVelocity(FlockAgent agent, ref List<Transform> detected)
        {
            //if no neighbors, return no adjustment
            if (detected.Count == 0)
            {
                return Vector3.zero;
            }

            Vector3 agentPosition = agent.transform.position._x0z();

            //add all points together and average
            Vector3 cohesionVelocity = Vector3.zero;
            
            foreach (Transform item in detected)
            {
                Vector3 itemPosition = item.position;
                cohesionVelocity += itemPosition;
            }
            cohesionVelocity /= detected.Count;

            //create offset from agent position
            cohesionVelocity -= agentPosition;
            return cohesionVelocity;
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
            colliders = new Collider[MAX_DETECT_COLLIDERS];
        }

        private void FixedUpdate() 
        {
            if(target == null)
            {
                Debug.LogError("Target is null!");
                return;
            }

            /*for(int i = 0; i < agents.Count; i++)
            {
                var current = agents[i];
                if (current == null || !current.gameObject.activeInHierarchy)
                {
                    continue;
                }

                if(Vector3.SqrMagnitude((target.position - current.transform.position)._x0z()) <= squareMinDistanceToTarget)
                {
                    nearThePlayer = true;
                    break;
                }
            }*/

            nearThePlayer = false;

            for(int i = 0; i < agents.Count; i++)
            {
                var current = agents[i];
                if(current == null || !current.gameObject.activeInHierarchy)
                {
                    continue;
                }

                Vector3 direction = (nearThePlayer == false) ? (target.position - current.transform.position).normalized._x0z() : Vector3.zero;

                var nearbyObjects = GetNearbyObjects(current);
                Vector3 avoidanceVelocity = avoidanceWeight * CalculateAvoidanceVelocity(current, ref nearbyObjects);
                Vector3 cohesionVelociety = Vector3.zero;
                if(allowSteeredCohesion)
                {
                    cohesionVelociety = CalculateSteeredCohesionVelocity(current, ref nearbyObjects);
                }
                else
                {
                    cohesionVelociety = CalculateCohesionVelocity(current, ref nearbyObjects);
                }

                
                Vector3 move = avoidanceVelocity + cohesionVelociety;
                move += direction;
                move *= driveFactor;
                
                if(move.sqrMagnitude > squareMaxSpeed)
                {
                    move = move.normalized * moveSpeed;
                }
                current.Move(move);
            }

            nearThePlayer = false;
        }

        private void OnValidate()
        {
            avoidanceRadius = neighborRadius * avoidanceRadiusMultiplier;
            squareMaxSpeed = moveSpeed * moveSpeed;
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
