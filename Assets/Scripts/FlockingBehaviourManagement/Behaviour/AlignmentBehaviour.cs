using System.Collections.Generic;
using UnityEngine;

namespace FlockingBehaviourManagement
{
    [CreateAssetMenu(menuName = "Flock/Behaviour/Alignment", fileName = "Alignment")]
    public class AlignmentBehaviour : FilteredFlockBehaviour
    {
        public override Vector3 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
        {
            if (context.Count == 0)
            {
                return agent.transform.forward;
            }
            

            //add all points together and do average
            Vector3 alignmentMove = Vector3.zero;
            List<Transform> filteredContext = (base.contextFilter == null) ? context : base.contextFilter.Filter(agent, context);
            foreach (Transform item in filteredContext)
            {
                alignmentMove += item.transform.forward;
            }
            alignmentMove /= context.Count;

            return alignmentMove;
        }
    }
}

