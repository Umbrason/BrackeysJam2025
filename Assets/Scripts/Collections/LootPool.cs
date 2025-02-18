using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LootPool<T>
{
    const float VarietyGuaranteeRatio = .5f;
    private readonly HashSet<T> Options = new();
    private readonly RingBuffer<T> LastLoot;
    public void Add(T item)
    {
        Options.Add(item);
        LastLoot.Resize(Mathf.FloorToInt(Options.Count * VarietyGuaranteeRatio));
    }
    public T Pull()
    {
        T item;
        var availableOptions = Options.Count - LastLoot.Count;
        if (availableOptions <= 0)
        {
            Debug.LogError("loot pool exhausted!");
            return default;
        }
        do
        {
            var idx = Random.Range(0, Options.Count);
            item = Options.ElementAt(idx);
        }
        while (LastLoot.Contains(item));
        LastLoot.Push(item);
        return item;
    }
    public void Remove(T item)
    {
        Options.Remove(item);
        LastLoot.Resize(Mathf.FloorToInt(Options.Count * VarietyGuaranteeRatio));
    }
}