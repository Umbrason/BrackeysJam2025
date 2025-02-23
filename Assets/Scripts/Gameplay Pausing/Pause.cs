using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    static readonly HashSet<Guid> pauseHandles = new();
    public static Guid Request()
    {
        var guid = Guid.NewGuid();
        pauseHandles.Add(guid);
        return guid;
    }

    public static void Return(Guid guid)
    {
        pauseHandles.Remove(guid);
    }

    void Awake()
    {
        pauseHandles.Clear();
        timescaleSpring = new(timescaleSpringConfig)
        {
            RestingPos = 1,
            Position = 1
        };
    }
    void OnDestroy() => pauseHandles.Clear();


    [SerializeField] private Spring.Config timescaleSpringConfig;
    BaseSpring timescaleSpring;

    void Update()
    {
        timescaleSpring.RestingPos = pauseHandles.Count > 0 ? 0 : 1;
        timescaleSpring.Step(Time.unscaledTime);
        Time.timeScale = Mathf.Clamp01(timescaleSpring.Position);
    }
}
