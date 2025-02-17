using System.Collections.Generic;
using UnityEngine;


namespace FlockingBehaviourManagement
{
    [CreateAssetMenu(menuName = "Flock/Behaviour/Avoidance", fileName = "Avoidance")]
    public class AvoidanceBehaviour : FilteredFlockBehaviour
    {
        public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
        {
            //if no neighbors, return no adjustment
            if (context.Count == 0)
                return Vector2.zero;

            //add all points together and average
            Vector2 avoidanceMove = Vector2.zero;
            int avoidCount = 0;
            List<Transform> filteredContext = (base.contextFilter == null) ? context : base.contextFilter.Filter(agent, context);
            foreach (Transform item in filteredContext)
            {
                if (Vector2.SqrMagnitude(item.position - agent.transform.position) < flock.SquareAvoidanceRadius)
                {
                    avoidCount++;
                    avoidanceMove += (Vector2)(agent.transform.position - item.position);
                }
            }
            if (avoidCount > 0)
                avoidanceMove /= avoidCount;

            return avoidanceMove;
        }
    }
}

