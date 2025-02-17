using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FlockingBehaviourManagement
{
    [CreateAssetMenu(menuName = "Flock/Filter/Phyics Layer Filter", fileName = "Phyics Layer Filter")]
    public class PhysicsLayerFilter : ContextFilter
    {
        public LayerMask layerMask;
        public override List<Transform> Filter(FlockAgent agent, List<Transform> original)
        {
            List<Transform> filtered = new List<Transform>();

            foreach(Transform item in original)
            {
                int itemLayerMask = 1 << item.gameObject.layer;
                if((layerMask & itemLayerMask) != 0)
                {
                    filtered.Add(item);
                }
            }

            return filtered;
        }
    }
}

