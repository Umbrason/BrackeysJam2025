using System.Collections.Generic;
using UnityEngine;

namespace FlockingBehaviourManagement
{
    [RequireComponent(typeof(Rigidbody))]
    public class FlockAgent : MonoBehaviour
    {
        private Collider agentCollider = null;
        private Rigidbody agentRB;
        private Flock flock = null;
        public Flock Flock { get => flock; }
        public Collider AgentCollider { get => agentCollider; }

        public void Initialize(Flock flock)
        {
            this.flock = flock;
        }
        public void Move(Vector3 velocity)
        {
            agentRB.velocity = velocity;
        }

        private void Awake() 
        {
            agentCollider = GetComponent<Collider>();
            agentRB = GetComponent<Rigidbody>();
        }


        private void Start() 
        {
            agentCollider.isTrigger = false;
            agentRB.isKinematic = false;
            agentRB.MoveRotation(Quaternion.Euler(0.0f, 0.0f, 0.0f));
            agentRB.constraints = RigidbodyConstraints.FreezePositionY |
                                  RigidbodyConstraints.FreezeRotationX | 
                                  RigidbodyConstraints.FreezeRotationY | 
                                  RigidbodyConstraints.FreezeRotationZ;
        }

    }
}
