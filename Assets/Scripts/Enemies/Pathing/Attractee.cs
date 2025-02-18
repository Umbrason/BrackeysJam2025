using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attractee : MonoBehaviour
{
    Cached<Rigidbody> cached_Rigidbody;
    Rigidbody RB => cached_Rigidbody[this];

    public float speed;

    void OnDrawGizmos()
    {
        Vector2 position = transform.position._xz();
        Vector2 vel = default;
        foreach (var attractor in Attractor.attractors)
            vel += attractor.GetInfluence(position);
        Gizmos.DrawLine(transform.position, transform.position + vel.normalized._x0y() * speed);
    }

    void FixedUpdate()
    {
        Vector2 position = transform.position._xz();
        Vector2 vel = default;
        foreach (var attractor in Attractor.attractors)
            vel += attractor.GetInfluence(position);
        RB.velocity = vel.normalized._x0y() * speed;
    }
}
