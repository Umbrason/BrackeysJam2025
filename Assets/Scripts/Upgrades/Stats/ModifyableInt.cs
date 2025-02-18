using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ModifyableInt
{
    public int Current { get => CalculateFormula(); }
    [field: SerializeField] public int Base { get; set; }
    private readonly Dictionary<Guid, int> additiveModifiers = new();
    private readonly Dictionary<Guid, float> multiplicativeModifiers = new();
    private readonly Dictionary<Guid, int> minModifiers = new();
    private readonly Dictionary<Guid, int> maxModifiers = new();

    private event Action<int> m_OnChanged;
    public event Action<int> OnChanged
    {
        add
        {
            m_OnChanged += value;
            value?.Invoke(Current);
        }
        remove => m_OnChanged -= value;
    }

    public ModifyableInt(int baseValue)
    {
        Base = baseValue;
    }

    private int CalculateFormula()
    {
        var result = (float)Base;

        foreach (var add in additiveModifiers.Values)
            result += add;

        foreach (var factor in multiplicativeModifiers.Values)
            result *= factor;

        foreach (var min in minModifiers.Values)
            result = result > min ? result : min;

        foreach (var max in maxModifiers.Values)
            result = result < max ? result : max;

        return Convert.ToInt32(result);
    }

    public Guid RegisterAdd(int value)
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

    public Guid RegisterFloor(int value)
    {
        var guid = Guid.NewGuid();
        minModifiers[guid] = value;
        m_OnChanged?.Invoke(Current);
        return guid;
    }

    public Guid RegisterCeil(int value)
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