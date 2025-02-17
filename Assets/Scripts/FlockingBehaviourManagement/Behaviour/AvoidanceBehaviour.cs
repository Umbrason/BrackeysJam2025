using System.Collections.Generic;
using UnityEngine;


namespace FlockingBehaviourManagement
{
    [CreateAssetMenu(menuName = "Flock/Behaviour/Avoidance", fileName = "Avoidance")]
    public class AvoidanceBehaviour : FilteredFlockBehaviour
    {
        public bool affectX = false;
        public bool affectY = false;
        public bool affectZ = false;

        public override Vector3 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
        {
            //if no neighbors, return no adjustment
            if (context.Count == 0)
            {
                return Vector3.zero;
            }

            Vector3 agentPosition = agent.transform.position;

            if (!affectX) agentPosition.x = 0;
            if (!affectY) agentPosition.y = 0;
            if (!affectZ) agentPosition.z = 0;

            //add all points together and average
            Vector3 avoidanceMove = Vector3.zero;
            int avoidCount = 0;
            List<Transform> filteredContext = (base.contextFilter == null) ? context : base.contextFilter.Filter(agent, context);
            foreach (Transform item in filteredContext)
            {
                Vector3 itemPosition = item.position;

                if (!affectX) itemPosition.x = 0;
                if (!affectY) itemPosition.y = 0;
                if (!affectZ) itemPosition.z = 0;

                if (Vector3.SqrMagnitude(itemPosition - agentPosition) < flock.SquareAvoidanceRadius)
                {
                    avoidCount++;
                    avoidanceMove += agent.transform.position - item.position;
                }
            }


            if (avoidCount > 0)
            {
                avoidanceMove /= avoidCount;
            }

            return avoidanceMove;
        }
    }
}

