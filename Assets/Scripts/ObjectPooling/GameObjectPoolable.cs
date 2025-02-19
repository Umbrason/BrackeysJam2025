using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectPoolable : MonoBehaviour, GameObjectPool.IPoolTarget
{
    public ObjectPool<GameObjectPoolable> Owner { get; set; }

    void ObjectPool.IPoolTarget.OnDespawn()
    {
        gameObject.SetActive(false);
    }

    void ObjectPool.IPoolTarget.OnSpawn()
    {
        gameObject.SetActive(true);
    }
}