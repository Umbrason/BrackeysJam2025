using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MusicPlayer : MonoBehaviour
{
    [SerializeField] private AudioClip startingClip;
    [SerializeField] private AudioMixerGroup audioMixerGroup;

    private AudioSource audioSource;

    public void Play(AudioClip clip)
    {
        Debug.Log("Playing music");
        if(audioSource.isPlaying)
        {
            audioSource.Stop();
        }
        audioSource.outputAudioMixerGroup = audioMixerGroup;
        audioSource.clip = clip;
        audioSource.loop = true;
        audioSource.playOnAwake = false;
        audioSource.Play();
    }

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.outputAudioMixerGroup = null;
        audioSource.clip = null;
        audioSource.loop = false;
        audioSource.playOnAwake = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        if(startingClip != null)
        {
            Play(startingClip);
        }
    }
}
