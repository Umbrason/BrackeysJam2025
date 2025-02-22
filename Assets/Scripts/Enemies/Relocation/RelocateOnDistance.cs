using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelocateOnDistance : MonoBehaviour
{
    Cached<EnemyPoolable> cached_Poolable;
    EnemyPoolable Poolable => cached_Poolable[this];

    Cached<SphereCollider> cached_Collider = new(Cached<SphereCollider>.GetOption.Children);
    SphereCollider Collider => cached_Collider[this];

    [SerializeField] float distance = 60;

    void FixedUpdate()
    {
        if ((RelocationCenter.Position - transform.position._xz()).sqrMagnitude <= distance * distance) return;
        var newPosition = ((EnemyPool)Poolable.Owner).Spawner.GenerateSpawnPosition(Collider.radius);
        transform.position = newPosition;
    }
}
