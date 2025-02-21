using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitSound : MonoBehaviour
{
    [SerializeField] private AudioClipGroup Light;
    [SerializeField] private AudioClipGroup Medium;
    [SerializeField] private AudioClipGroup Severe;

    enum HealthLoss
    {
        Light = 0,
        Medium = 35,
        Severe = 75,
    }

    private AudioClipGroup GetAudioGroup(int percentageLost)
    {
        if (percentageLost >= (int)HealthLoss.Severe) return Severe;
        else if (percentageLost >= (int)HealthLoss.Medium) return Medium;
        else return Light;
    }

    Cached<HealthPool> cached_healthPool;
    HealthPool HealthPool => cached_healthPool[this];

    void Awake()
    {
        HealthPool.OnModified += OnHealthModified;
    }

    void OnDestroy()
    {
        if (HealthPool) HealthPool.OnModified -= OnHealthModified;
    }

    void OnHealthModified(int change)
    {
        if (change >= 0) return;
        change = Mathf.FloorToInt(-change * 100f / HealthPool.Size); //change to percentage value
        var audioGroup = GetAudioGroup(change);
        SFXPool.PlayAt(audioGroup, transform.position);
    }
}
