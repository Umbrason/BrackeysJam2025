using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FlockingBehaviourManagement
{
    [CreateAssetMenu(menuName = "Flock/Filter/Same Flock", fileName = "Same Filter")]
    public class SameFlockFilter : ContextFilter
    {
        public override List<Transform> Filter(FlockAgent agent, List<Transform> original)
        {
            List<Transform> filtered = new List<Transform>();
            foreach (Transform item in original)
            {
                FlockAgent itemAgent = item.GetComponent<FlockAgent>();
                if (itemAgent != null && itemAgent.Flock == agent.Flock)
                {
                    filtered.Add(item);
                }
            }
            return filtered;
        }
    }
}

