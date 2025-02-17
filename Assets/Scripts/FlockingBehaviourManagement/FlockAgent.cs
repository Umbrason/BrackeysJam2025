using System.Collections;
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
        private static List<FlockAgent> allFlockAgents = new List<FlockAgent>();

        public static List<FlockAgent> AllFlockAgents { get => allFlockAgents; }
        public Flock Flock { get => flock; }

        public void Initialize(Flock flock)
        {
            this.flock = flock;
        }
        public void Move(Vector2 velocity)
        {
            transform.up = velocity;
            agentRB.MovePosition(agentRB.position + velocity._x0y() * Time.fixedDeltaTime);
        }

        private void Awake() 
        {
            agentCollider = GetComponent<Collider>();
            agentRB = GetComponent<Rigidbody>();
        }

        private void OnEnable() 
        {
            allFlockAgents.Add(this);
        }

        private void Start() 
        {
            agentCollider.isTrigger = true;
            agentRB.isKinematic = true;    
        }

        private void OnDisable() 
        {
            allFlockAgents.Remove(this);
        }


    }

}
