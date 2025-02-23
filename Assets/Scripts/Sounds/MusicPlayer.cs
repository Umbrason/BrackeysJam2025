using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MusicPlayer : MonoBehaviour
{
    [Header("Clips")]
    [SerializeField] private AudioClip calmStartingClip;
    [SerializeField] private AudioClip calmLoopingClip;
    [SerializeField] private AudioClip exitingStartingClip;
    [SerializeField] private AudioClip exitingLoopingClip;

    [Header("Refs")]
    [SerializeField] private AudioSource calmAudioSource;
    [SerializeField] private AudioSource exitingAudioSource;
    [SerializeField] private EnemySpawner enemySpawner;

    [SerializeField] private Spring.Config CrossfadeSpringConfig = new(5, 1);
    BaseSpring CrossFadeSpring;


    public void Play(AudioClip clip, AudioSource source)
    {
        if (source.isPlaying)
        {
            calmAudioSource.Stop();
        }
        source.clip = clip;
        source.Play();
    }

    private void Awake()
    {
        CrossFadeSpring = new(CrossfadeSpringConfig)
        {
            Position = 0,
            RestingPos = 0
        };
        StartCoroutine(Playback());
    }

    IEnumerator Playback()
    {
        Play(calmStartingClip, calmAudioSource);
        Play(exitingStartingClip, exitingAudioSource);
        yield return new WaitUntil(() => !calmAudioSource.isPlaying);
        calmAudioSource.clip = calmLoopingClip;
        Play(calmLoopingClip, calmAudioSource);
        Play(exitingLoopingClip, exitingAudioSource);
        calmAudioSource.loop = true;
        exitingAudioSource.loop = true;
    }
    void Update()
    {
        if(enemySpawner != null) CrossFadeSpring.RestingPos = Mathf.Clamp01((enemySpawner.EnemyCount - enemySpawner.DesiredEnemyCount)  * 2f / enemySpawner.DesiredEnemyCount); //1 => exiting
        CrossFadeSpring.Step(Time.deltaTime);
        calmAudioSource.volume = 1 - CrossFadeSpring.Position;
        exitingAudioSource.volume = CrossFadeSpring.Position;
    }
}
