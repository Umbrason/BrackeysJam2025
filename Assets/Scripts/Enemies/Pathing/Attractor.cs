
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-1)]
public class Attractor : MonoBehaviour
{
    public static List<Attractor> attractors = new();

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

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        if (falloffType == 0) return;
        var r = Mathf.Pow(Mathf.Abs(strength), 1f / (int)falloffType);
        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.up, r);
    }
#endif

    public void FixedUpdate()
    {
        cachedX = transform.position.x;
        cachedY = transform.position.z;
    }

    float cachedX;
    float cachedY;
    public (float x, float y) GetInfluence(float x, float y)
    {
        var (dx, dy) = (cachedX - x, cachedY - y);
        var reciprocalMag = 1f / (float)Math.Sqrt(dx * dx + dy * dy);
        if (reciprocalMag == float.NaN || reciprocalMag > 100) return default;
        dx *= reciprocalMag;
        dy *= reciprocalMag;
        reciprocalMag = (float)Math.Pow(reciprocalMag, (int)falloffType);
        var c = reciprocalMag * strength;
        return (dx * c, dy * c);
    }

}
