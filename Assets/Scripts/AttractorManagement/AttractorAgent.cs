using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AttractorManagement
{
    [RequireComponent(typeof(Rigidbody))]
    public class AttractorAgent : MonoBehaviour
    {
        private const int MAX_ATTRACTORS_DETECT_COUNT = 10;
        [SerializeField] private LayerMask attractorLayerMask;
        [Min(0.1f)]
        [SerializeField] private float detectRadius = 2.0f;
        [Min(0.1f)]
        [SerializeField] private float calculatingVelocityInterval = 0.5f;
        [Min(0.1f)]
        [SerializeField] private float maxSpeed = 20.0f;
        [SerializeField] private Transform target;

        private Collider agentCollider;
        private Rigidbody agentRB;

        private Collider[] colliders = null;
        private Attractor targetAttractor = null;
        private Coroutine calculateVelCoroutine = null;
        private Vector3 calculatedVelocity = Vector3.zero;

        public Transform Target { get => target; set => target = value; }

        private IEnumerator CalculateVelocity()
        {
            if(target != null)
            {
                if(!target.TryGetComponent<Attractor>(out targetAttractor))
                {
                    Debug.LogError("Cannot find attractor on Target!");
                }
            }
            while(Application.isPlaying)
            {
                int count = Physics.OverlapSphereNonAlloc(transform.position, 
                                                          detectRadius, 
                                                          colliders, 
                                                          attractorLayerMask.value);
                
                //Debug.Log("Count: " + count);

                Attractor attractor = null;

                Vector3 agentPosition = transform.position._x0z();

                Vector3 totalVelocity = Vector3.zero;

                Vector3 attractorPosition = Vector3.zero;
                Vector3 direction = Vector3.zero;
                float distance = 0.0f;
                float falloff = 0.0f;
                Vector3 velocity = Vector3.zero;

                attractorPosition = targetAttractor.transform.position._x0z();
                direction = (attractorPosition - agentPosition).normalized;
                distance = Vector3.Distance(agentPosition, attractorPosition);
                falloff = targetAttractor.GetFalloff(distance);

                velocity = (direction * targetAttractor.Strength) / falloff;
                Debug.Log("Velocity magnitude for target: " + velocity.magnitude);
                totalVelocity += velocity;

                Debug.DrawLine(transform.position, transform.position + velocity, Color.red);

                for(int i = 0; i < count; i++)
                {
                    var current = colliders[i];
                    if(current.transform == target)
                    {
                        yield return null;
                        continue;
                    }
                    else if(current.transform.TryGetComponent<Attractor>(out attractor))
                    {
                        attractorPosition = attractor.transform.position._x0z();
                        direction = -(attractorPosition - agentPosition).normalized;
                        
                        distance = Vector3.Distance(agentPosition, attractorPosition);

                        falloff = attractor.GetFalloff(distance);
                        velocity = (direction * attractor.Strength) / falloff;

                        Debug.DrawLine(transform.position, transform.position + velocity, Color.yellow);
                        Debug.Log("Velocity magnitude: " + velocity.magnitude);
                        totalVelocity += velocity;
                    }
                }

                Debug.Log("Total vel magnitude before clamping: " + totalVelocity.magnitude);
                totalVelocity = Vector3.ClampMagnitude(totalVelocity, maxSpeed);
                //Debug.Log("Total vel magnitude after clamping: " + totalVelocity.magnitude);

                calculatedVelocity = totalVelocity;
                yield return new WaitForSeconds(calculatingVelocityInterval);
            }
        }

        private void Move()
        {
            agentRB.velocity = calculatedVelocity;
        }
        private void Awake()
        {
            agentRB = GetComponent<Rigidbody>();
            agentCollider = GetComponent<Collider>();
            colliders = new Collider[MAX_ATTRACTORS_DETECT_COUNT];
        }

        // Start is called before the first frame update
        void Start()
        {
            agentCollider.isTrigger = false;
            agentRB.isKinematic = false;
            agentRB.MoveRotation(Quaternion.Euler(0.0f, 0.0f, 0.0f));
            agentRB.constraints = RigidbodyConstraints.FreezePositionY |
                                  RigidbodyConstraints.FreezeRotationX |
                                  RigidbodyConstraints.FreezeRotationY |
                                  RigidbodyConstraints.FreezeRotationZ;

            if(calculateVelCoroutine != null)
            {
                StopCoroutine(calculateVelCoroutine);
            }

            calculateVelCoroutine = StartCoroutine(CalculateVelocity());
        }

        
        void FixedUpdate()
        {
            Move();
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, detectRadius);
        }
    }
}

