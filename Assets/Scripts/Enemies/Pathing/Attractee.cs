using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Attractee : MonoBehaviour
{
    Cached<Rigidbody> cached_Rigidbody;
    Rigidbody RB => cached_Rigidbody[this];

    public float speed;

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
            var (avx, avy) = attractor.GetInfluence(px, py);
            vx += avx;
            vy += avy;
        }
        RB.velocity = new Vector3(vx, 0, vy).normalized * speed;
    }
}
