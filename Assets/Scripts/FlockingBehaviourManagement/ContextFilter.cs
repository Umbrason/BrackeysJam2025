using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace FlockingBehaviourManagement
{
    public abstract class ContextFilter : ScriptableObject
    {
        public abstract List<Transform> Filter(FlockAgent agent, List<Transform> original);
    }

}
