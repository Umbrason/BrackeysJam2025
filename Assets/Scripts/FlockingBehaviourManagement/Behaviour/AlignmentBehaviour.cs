using System.Collections.Generic;
using UnityEngine;

namespace FlockingBehaviourManagement
{
    [CreateAssetMenu(menuName = "Flock/Behaviour/Alignment", fileName = "Alignment")]
    public class AlignmentBehaviour : FilteredFlockBehaviour
    {
        public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
        {
            if (context.Count == 0)
            return agent.transform.up;

            //add all points together and do average
            Vector2 alignmentMove = Vector2.zero;
            List<Transform> filteredContext = (base.contextFilter == null) ? context : base.contextFilter.Filter(agent, context);
            foreach (Transform item in filteredContext)
            {
                alignmentMove += (Vector2)item.transform.up;
            }
            alignmentMove /= context.Count;

            return alignmentMove;
        }
    }
}

