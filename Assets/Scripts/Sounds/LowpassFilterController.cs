using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class LowpassFilterController : MonoBehaviour
{
    private const string CUTOFF_KEY = "cutoff_freq";

    [SerializeField] private AudioMixerGroup audioMixerGroup;
    [Min(500.0f)]
    [SerializeField] private float defaultCutoffFreq = 5000.0f;
    [Min(20.0f)]
    [SerializeField] private float minCutoffFreq = 200.0f;

    private float previousWeight = 0.0f;
    private void HandleCutoffFreq()
    {
        float weight = 1.0f - Time.timeScale;
        
        if(previousWeight == weight)
        {
            return;
        }

        float lerpedCuttoffFreq = Mathf.Lerp(defaultCutoffFreq, minCutoffFreq, weight);
        audioMixerGroup.audioMixer.SetFloat(CUTOFF_KEY, lerpedCuttoffFreq);
        previousWeight = weight;
    }

    private void Update()
    {
        HandleCutoffFreq();
    }

    private void OnDestroy()
    {
        audioMixerGroup.audioMixer.SetFloat(CUTOFF_KEY, defaultCutoffFreq);
    }
}
