using System;
using System.Collections.Generic;
using UnityEngine;

public class HealthPool : MonoBehaviour
{
    [field: SerializeField] public int Size { get; private set; }
    [field: SerializeField] public int Current { get; private set; }
    public event Action OnModified;
    public event Action OnDepleted;
    void FixedUpdate()
    {
        ProcessHealthEvents();
    }
    public void OnEnable() => Current = Size;

    private readonly Dictionary<Guid, (HealthEvent healthEvent, int multiplier)> unprocessedDamageEvents = new();
    private readonly HashSet<Guid> processedDamageEvents = new();
    public void RegisterHealthEvent(HealthEvent healthEvent, int multiplier = 1)
    {
        if (processedDamageEvents.Contains(healthEvent.GUID)) return;
        var existingMultiplier = unprocessedDamageEvents.ContainsKey(healthEvent.GUID) ? unprocessedDamageEvents[healthEvent.GUID].multiplier : 0;
        unprocessedDamageEvents[healthEvent.GUID] = (healthEvent, Mathf.Max(existingMultiplier, multiplier));
    }

    public void Resize(int newSize)
    {
        Current = Mathf.Min(newSize, Current);
        Size = newSize;
        OnModified?.Invoke();
        if (Current == 0)
            OnDepleted?.Invoke();
    }

    private void ProcessHealthEvents()
    {
        foreach ((var healthEvent, var multiplier) in unprocessedDamageEvents.Values)
        {
            processedDamageEvents.Add(healthEvent.GUID);
            healthEvent.OnReleased += () => processedDamageEvents.Remove(healthEvent.GUID);
            this.Current += healthEvent.Amount * multiplier;
        }
        this.Current = Mathf.Clamp(Current, 0, Size);
        foreach ((var healthEvent, var multiplier) in unprocessedDamageEvents.Values)
            healthEvent.ReportHit(this, healthEvent.Amount * multiplier, this.Current == 0);
        unprocessedDamageEvents.Clear();
        OnModified?.Invoke();
        if (Current == 0) OnDepleted?.Invoke();
    }
}