using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleVoicelinePlayer : MonoBehaviour
{
    [SerializeField] float minDelay = 10;
    [SerializeField] float maxDelay = 20;
    [SerializeField] AudioClipGroup idleVoicelines;

    void Start()
    {
        StartCoroutine(IdleVoicelineRoutine());
    }

    private IEnumerator IdleVoicelineRoutine()
    {
        while (true)
        {
            var delay = UnityEngine.Random.Range(minDelay, maxDelay);
            yield return new WaitForSeconds(delay);
            VoicelinePlayer.Play(idleVoicelines);
        }
    }
}
