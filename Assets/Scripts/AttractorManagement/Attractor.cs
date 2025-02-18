using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AttractorManagement
{
    public class Attractor : MonoBehaviour
    {
        private const float MIN_DISTANCE = 0.1f;
        [SerializeField] private float strength = 10.0f;
        [SerializeField] private FalloffType falloffType = FalloffType.Linear;
        public float Strength { get => strength; }

        public enum FalloffType
        {
            None = -1,
            Linear,
            Quadratic,
            Cubic
        }
        
        public float GetFalloff(float distance)
        {
            switch(falloffType)
            {
                case FalloffType.Linear: 
                            return strength / distance;
                    

                case FalloffType.Quadratic:
                            if(Mathf.Abs(distance) <= MIN_DISTANCE)
                            {
                                break;
                            }

                            return strength / (distance * distance);

                case FalloffType.Cubic:
                            if (Mathf.Abs(distance) <= MIN_DISTANCE)
                            {
                                break;
                            }

                            return strength / (distance * distance * distance);
            }

            return strength;
        }
    }
}
