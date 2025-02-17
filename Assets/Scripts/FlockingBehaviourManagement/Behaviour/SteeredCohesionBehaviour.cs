using System.Collections.Generic;
using UnityEngine;


namespace FlockingBehaviourManagement
{
    public class SteeredCohesionBehaviour : FilteredFlockBehaviour
    {
        public float agentSmoothTime = 0.5f;
        private Vector3 currentVelocity;
    
        public override Vector3 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
        {
            //if no neighbors, return no adjustment
            if (context.Count == 0)
            {
                return Vector3.zero;
            }
                
            //add all points together and average
            Vector3 cohesionMove = Vector3.zero;
            List<Transform> filteredContext = (base.contextFilter == null) ? context : base.contextFilter.Filter(agent, context);
            foreach (Transform item in filteredContext)
            {
                cohesionMove += item.position;
            }
            cohesionMove /= context.Count;

            //create offset from agent position
            cohesionMove -= agent.transform.position;
            cohesionMove = Vector3.SmoothDamp(agent.transform.up, cohesionMove, ref currentVelocity, agentSmoothTime);
            return cohesionMove;
        }
    }
}

