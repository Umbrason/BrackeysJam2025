using System.Collections.Generic;
using UnityEngine;

public class LootPool<T>
{
    const float VarietyGuaranteeRatio = .5f;
    IReadOnlyCollection<T> Options;
    RingBuffer<T> LastLoot => new(Mathf.FloorToInt(VarietyGuaranteeRatio * Options.Count));
    public LootPool(IReadOnlyCollection<T> options)
    {
        Options = options;
    }
}