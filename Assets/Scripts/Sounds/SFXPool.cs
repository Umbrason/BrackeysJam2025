using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SFXPool : MonoBehaviour
{
    public static readonly Dictionary<AudioClipGroup, SFXPool> Instances = new();
    [SerializeField] AudioClipGroup group;
    [SerializeField] private int Size;
    [SerializeField] private AudioSource Template;
    private readonly RingBuffer<AudioSource> audioSources = new(0);

    void Awake()
    {
        audioSources.Resize(Size);
        for (int i = 0; i < Size; i++)
            CreateAudioSource();
    }

    public static void PlayAt(AudioClipGroup group, Vector3 position)
    {
        if (!Instances.TryGetValue(group, out var pool))
        {
            Debug.LogError($"No Audio Pool found for {group.name}");
            return;
        }
        pool.PlayAt(position);
    }

    void OnEnable() => Instances.Add(group, this);
    void OnDisable() => Instances.Remove(group);

    void CreateAudioSource()
    {
        var AS = Instantiate(Template, transform);
        AS.gameObject.SetActive(false);
        AS.outputAudioMixerGroup = group.MixerGroup;
        audioSources.Push(AS);
    }

    const float minPlayDelay = .025f;
    const float interrupt = .05f;
    float lastTimePlayed;
    public void PlayAt(Vector3 position)
    {
        if (audioSources.NextValue.gameObject.activeSelf) //check if advanced enough to interrput
        {
            var progress = audioSources.NextValue.time / audioSources.NextValue.clip.length;
            if (progress < interrupt) return;
        }
        if (Time.unscaledTime - lastTimePlayed < minPlayDelay) return;
        lastTimePlayed = Time.unscaledTime;


        if (!group.TryGetRandom(out var clip)) return;
        var AS = audioSources.Pop();
        if (AS.isPlaying) AS.Stop();
        AS.pitch = group.RandomPitch();
        AS.transform.position = position;
        AS.clip = clip;
        AS.gameObject.SetActive(true);
        AS.Play();
        audioSources.Push(AS);
        StartCoroutine(DisableAfterPlaying(AS));
    }

    IEnumerator DisableAfterPlaying(AudioSource AS)
    {
        yield return new WaitUntil(() => !AS.isPlaying);
        AS.gameObject.SetActive(false);
    }
}
