using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoicelinePlayer : MonoBehaviour
{
    private static VoicelinePlayer Instance;

    private float lastVoicelineTime;
    void OnEnable() => Instance = this;
    void OnDisable() => Instance = null;

    public static void Play(AudioClipGroup voiceLine, bool forcePlace = false) => Instance.PlayFromInstance(voiceLine, forcePlace);
    [SerializeField] AudioSource source;
    private void PlayFromInstance(AudioClipGroup group, bool forcePlay = false)
    {
        if (group == null) return;
        if (Time.time - lastVoicelineTime < 3 & !forcePlay) return;
        lastVoicelineTime = Time.time;
        if (group.TryGetRandom(out var clip))
        {
            source.clip = clip;
            source.Play();
        }
    }
}
