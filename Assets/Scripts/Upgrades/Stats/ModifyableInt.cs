using System;
using System.Collections.Generic;

public class ModifyableInt
{
    public int Current { get => CalculateFormula(); }
    public int Base { get; set; }
    private readonly Dictionary<Guid, int> additiveModifiers = new();
    private readonly Dictionary<Guid, int> multiplicativeModifiers = new();
    private readonly Dictionary<Guid, int> minModifiers = new();
    private readonly Dictionary<Guid, int> maxModifiers = new();

    public ModifyableInt(int baseValue)
    {
        Base = baseValue;
    }

    private int CalculateFormula()
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

    public Guid RegisterAdd(int value)
    {
        var guid = Guid.NewGuid();
        additiveModifiers[guid] = value;
        return guid;
    }

    public Guid RegisterMultiply(int value)
    {
        var guid = Guid.NewGuid();
        multiplicativeModifiers[guid] = value;
        return guid;
    }

    public Guid RegisterFloor(int value)
    {
        var guid = Guid.NewGuid();
        minModifiers[guid] = value;
        return guid;
    }

    public Guid RegisterCeil(int value)
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