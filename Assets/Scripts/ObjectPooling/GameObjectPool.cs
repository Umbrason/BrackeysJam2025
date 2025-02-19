using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectPool : ObjectPool<GameObjectPoolable>
{
    public void Pull(Vector3 position, Quaternion rotation)
    {
        var go = this.Pull();
        go.gameObject.transform.SetPositionAndRotation(position, rotation);
    }

    public override void OnCreate(GameObjectPoolable instance)
    {
        instance.gameObject.SetActive(false);
    }
}
