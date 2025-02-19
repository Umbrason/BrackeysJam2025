using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieOnTime : MonoBehaviour
{
    Cached<ObjectPool.IPoolTarget> cached_PoolTarget;
    ObjectPool.IPoolTarget PoolTarget => cached_PoolTarget[this];

    [SerializeField] private float lifeTime;
    private float timeOfBirth;
    void OnEnable()
    {
        timeOfBirth = Time.time;
    }

    void Update()
    {
        if (Time.time >= timeOfBirth + lifeTime) //time of death
            PoolTarget.Despawn();
    }
}
