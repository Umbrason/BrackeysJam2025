using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FlockingBehaviourManagement
{
    [System.Serializable]
    public class FlockData
    {
        [Min(0.0f)]
        public float agentDensity = 0.08f;
        
        [Range(1f, 100f)]
        public float moveSpeed = 5f;
        
        [Range(1f, 10f)]
        public float neighborRadius = 1.5f;
        
        [Range(0f, 1f)]
        public float avoidanceRadiusMultiplier = 0.5f;
        
        [Range(0.0f, 1.0f)]
        public float avoidanceWeight = 1.0f;

        [Range(0.0f, 1.0f)]
        public float cohesionWeight = 1.0f;

        [Min(0.1f)]
        public float agentSmoothTime = 0.5f;
        
        public bool allowSteeredCohesion = true;
        
        public LayerMask obstacleLayerMask;
        
        [Min(1.0f)]
        public float minDistanceToTarget = 5.0f;
    }

}
