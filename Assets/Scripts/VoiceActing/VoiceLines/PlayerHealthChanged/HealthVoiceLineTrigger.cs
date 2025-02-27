using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthVoiceLineTrigger : MonoBehaviour
{
    [SerializeField] AudioClipGroup healed;
    [SerializeField] AudioClipGroup hurt;
    [SerializeField] AudioClipGroup lowHP;
    [SerializeField] AudioClipGroup died;
    float lowThreshold = .3f;
    [SerializeField] float probability = 1f;

    Cached<HealthPool> cached_HealthPool;
    HealthPool HealthPool => cached_HealthPool[this];
    void Start()
    {
        HealthPool.OnModified += OnModified;
        HealthPool.OnDepleted += () => { if (Random.value <= probability) VoicelinePlayer.Play(died); };
    }

    void OnDestroy()
    {
        HealthPool.OnModified += OnModified;
    }

    void OnModified(int amount)
    {
        if (Random.value > probability) return;
        if (amount < 0)
        {
            var wasAboveThreshold = (HealthPool.Current - amount) / (float)HealthPool.Size > lowThreshold;
            var isBelowThreshold = (HealthPool.Current) / (float)HealthPool.Size < lowThreshold;
            if (wasAboveThreshold && isBelowThreshold) VoicelinePlayer.Play(lowHP, true);
            else VoicelinePlayer.Play(hurt);
        }
        if (amount > 0)
            VoicelinePlayer.Play(healed);
    }
}
