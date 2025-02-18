using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ModifyableFloat
{
    public float Current { get => CalculateFormula(); }
    [field: SerializeField] public float Base { get; set; }
    private readonly Dictionary<Guid, float> additiveModifiers = new();
    private readonly Dictionary<Guid, float> multiplicativeModifiers = new();
    private readonly Dictionary<Guid, float> minModifiers = new();
    private readonly Dictionary<Guid, float> maxModifiers = new();

    private event Action<float> m_OnChanged;
    public event Action<float> OnChanged
    {
        add
        {
            m_OnChanged += value;
            value?.Invoke(Current);
        }
        remove => m_OnChanged -= value;
    }

    public ModifyableFloat(float baseValue)
    {
        Base = baseValue;
    }

    private float CalculateFormula()
    {
        var result = Base;

        foreach (var add in additiveModifiers.Values)
            result += add;

        foreach (var factor in multiplicativeModifiers.Values)
            result *= factor;

        foreach (var min in minModifiers.Values)
            result = result > min ? result : min;

        foreach (var max in maxModifiers.Values)
            result = result < max ? result : max;

        return result;
    }

    public Guid RegisterAdd(float value)
    {
        var guid = Guid.NewGuid();
        additiveModifiers[guid] = value;
        m_OnChanged?.Invoke(Current);
        return guid;
    }

    public Guid RegisterMultiply(float value)
    {
        var guid = Guid.NewGuid();
        multiplicativeModifiers[guid] = value;
        m_OnChanged?.Invoke(Current);
        return guid;
    }

    public Guid RegisterFloor(float value)
    {
        var guid = Guid.NewGuid();
        minModifiers[guid] = value;
        m_OnChanged?.Invoke(Current);
        return guid;
    }

    public Guid RegisterCeil(float value)
    {
        var guid = Guid.NewGuid();
        maxModifiers[guid] = value;
        m_OnChanged?.Invoke(Current);
        return guid;
    }

    public void FreeAdd(Guid guid) { additiveModifiers.Remove(guid); m_OnChanged?.Invoke(Current); }
    public void FreeMultiply(Guid guid) { multiplicativeModifiers.Remove(guid); m_OnChanged?.Invoke(Current); }
    public void FreeFloor(Guid guid) { minModifiers.Remove(guid); m_OnChanged?.Invoke(Current); }
    public void FreeCeil(Guid guid) { maxModifiers.Remove(guid); m_OnChanged?.Invoke(Current); }
}