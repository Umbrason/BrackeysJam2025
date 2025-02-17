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
        public FlockBehaviour behaviour = null;
        [Range(1f, 100f)]
        public float driveFactor = 10f;
        [Range(1f, 100f)]
        public float maxSpeed = 5f;
        [Range(1f, 10f)]
        public float neighborRadius = 1.5f;
        [Range(0f, 1f)]
        public float avoidanceRadiusMultiplier = 0.5f;

    }

}
