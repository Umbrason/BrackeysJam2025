using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class AudioClipGroup : ScriptableObject
{
    [field: SerializeField] public List<AudioClip> Clips { get; private set; } = new();

    public bool TryGetRandom(out AudioClip clip)
    {
        if (Clips == null || Clips.Count == 0)
        {
            clip = null;
            return false;
        }

        clip = Clips[Random.Range(0, Clips.Count)];
        return true;
    }
}