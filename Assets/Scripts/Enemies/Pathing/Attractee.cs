using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Attractee : MonoBehaviour
{
    Cached<VelocityController> cached_VelocityController;
    VelocityController VC => cached_VelocityController[this];

    public float speed;
    [SerializeField] private int attractionLayerMask;

    void OnDrawGizmos()
    {
        Vector3 position = transform.position;
        float px = position.x;
        float py = position.z;
        float vx = 0;
        float vy = 0;

        for (int i = 0; i < Attractor.attractors.Count; i++)
        {
            var attractor = Attractor.attractors[i];
            var (avx, avy) = attractor.GetInfluence(px, py);
            vx += avx;
            vy += avy;
        }
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(vx, 0, vy).normalized * speed);
    }

    void FixedUpdate()
    {
        Vector3 position = transform.position;
        float px = position.x;
        float py = position.z;
        float vx = 0;
        float vy = 0;

        for (int i = 0; i < Attractor.attractors.Count; i++)
        {
            var attractor = Attractor.attractors[i];
            if (((1 << attractor.AttractionLayer) & attractionLayerMask) == 0) continue;
            var (avx, avy) = attractor.GetInfluence(px, py);
            vx += avx;
            vy += avy;
        }
        VC.AddOverwriteMovement(new(new Vector3(vx, 0, vy).normalized * speed), 0, 0);
    }
}
