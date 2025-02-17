using System.Collections.Generic;
using UnityEngine;


namespace FlockingBehaviourManagement
{
    public class SteeredCohesionBehaviour : FilteredFlockBehaviour
    {
        public float agentSmoothTime = 0.5f;
        private Vector2 currentVelocity;
    
        public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
        {
            //if no neighbors, return no adjustment
            if (context.Count == 0)
                return Vector2.zero;

            //add all points together and average
            Vector2 cohesionMove = Vector2.zero;
            List<Transform> filteredContext = (base.contextFilter == null) ? context : base.contextFilter.Filter(agent, context);
            foreach (Transform item in filteredContext)
            {
                cohesionMove += (Vector2)item.position;
            }
            cohesionMove /= context.Count;

            //create offset from agent position
            cohesionMove -= (Vector2)agent.transform.position;
            cohesionMove = Vector2.SmoothDamp(agent.transform.up, cohesionMove, ref currentVelocity, agentSmoothTime);
            return cohesionMove;
        }
    }
}

