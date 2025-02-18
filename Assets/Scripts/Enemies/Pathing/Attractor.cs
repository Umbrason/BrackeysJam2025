
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attractor : MonoBehaviour
{
    public static HashSet<Attractor> attractors = new();
    public Vector2 Position => transform.position._xz();
    public enum Falloff
    {
        None = 0,
        Linear = 1,
        Quadtratic = 2,
        Cubic = 3
    }
    public Falloff falloffType;
    public float strength;

    public void OnEnable() => attractors.Add(this);
    public void OnDisable() => attractors.Remove(this);

    public Vector2 GetInfluence(Vector2 position)
    {
        var diff = this.Position - position;
        var mag = 1 / diff.magnitude;
        diff *= mag;
        mag = Mathf.Pow(mag, (int)falloffType);
        return mag * strength * diff;
    }

}
