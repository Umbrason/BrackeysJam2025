using System;
using System.Collections.Generic;

public class ModifyableFloat
{
    public float Current { get => CalculateFormula(); }
    public float Base { get; set; }
    private readonly Dictionary<Guid, float> additiveModifiers = new();
    private readonly Dictionary<Guid, float> multiplicativeModifiers = new();
    private readonly Dictionary<Guid, float> minModifiers = new();
    private readonly Dictionary<Guid, float> maxModifiers = new();

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
        return guid;
    }

    public Guid RegisterMultiply(float value)
    {
        var guid = Guid.NewGuid();
        multiplicativeModifiers[guid] = value;
        return guid;
    }

    public Guid RegisterFloor(float value)
    {
        var guid = Guid.NewGuid();
        minModifiers[guid] = value;
        return guid;
    }

    public Guid RegisterCeil(float value)
    {
        var guid = Guid.NewGuid();
        maxModifiers[guid] = value;
        return guid;
    }

    public void FreeAdd(Guid guid) => additiveModifiers.Remove(guid);
    public void FreeMultiply(Guid guid) => multiplicativeModifiers.Remove(guid);
    public void FreeFloor(Guid guid) => minModifiers.Remove(guid);
    public void FreeCeil(Guid guid) => maxModifiers.Remove(guid);
}