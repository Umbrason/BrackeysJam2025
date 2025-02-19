using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> : MonoBehaviour where T : Object, ObjectPool<T>.IPoolTarget
{
    [SerializeField] private int prewarmCount;
    public interface IPoolTarget : ObjectPool.IPoolTarget
    {
        ObjectPool<T> Owner { get; set; }
        void ObjectPool.IPoolTarget.Despawn() => Owner.Return((T)this);
    }
    public readonly List<T> instances = new();
    public int InCirculation { get; private set; }
    public T Template;
    void Awake()
    {
        for (int i = 0; i < prewarmCount; i++)
            Create();
    }

    private void Create()
    {
        var instance = Instantiate(Template, transform);
        instance.Owner = this;
        instances.Add(instance);
        OnCreate(instance);
    }

    public virtual void OnCreate(T instance) { }

    public T Pull()
    {
        if (instances.Count == 0)
            Create();
        var instance = instances[^1];
        instances.RemoveAt(instances.Count - 1);
        instance.OnSpawn();
        InCirculation++;
        return instance;
    }

    public void Return(T instance)
    {
        instance.OnDespawn();
        instances.Add(instance);
        InCirculation--;
    }
}

public class ObjectPool
{
    public interface IPoolTarget
    {
        internal void OnSpawn();
        internal void OnDespawn();
        void Despawn();
    }
}