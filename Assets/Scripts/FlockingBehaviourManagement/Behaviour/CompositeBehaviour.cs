using System.Collections.Generic;
using UnityEngine;

namespace FlockingBehaviourManagement
{
    [CreateAssetMenu(menuName = "Flock/Behaviour/Composite", fileName = "Composite")]
    public class CompositeBehaviour : FlockBehaviour
    {
        [SerializeField]private FlockBehaviour[] behaviours;
        [SerializeField]private float[] weights;
        public override Vector3 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
        {
            if(weights.Length != behaviours.Length)
            {
                Debug.LogError("Data mismatch in " + base.name);
                return Vector3.zero;
            }

            //set up move
            Vector3 move = Vector3.zero;

            //iterate through behaviors
            for (int i = 0; i < behaviours.Length; i++)
            {
                Vector3 partialMove = behaviours[i].CalculateMove(agent, context, flock) * weights[i];

                if (partialMove != Vector3.zero)
                {
                    if (partialMove.sqrMagnitude > weights[i] * weights[i])
                    {
                        partialMove.Normalize();
                        partialMove *= weights[i];
                    }

                    move += partialMove;

                }
            }

            return move;
        }
    }
}

