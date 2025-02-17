using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FlockingBehaviourManagement
{
    public abstract class FilteredFlockBehaviour : FlockBehaviour
    {
        public ContextFilter contextFilter;
    }
}

