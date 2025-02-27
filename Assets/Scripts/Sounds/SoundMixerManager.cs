﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;

// https://www.youtube.com/watch?v=DU7cgVsU2rM&t=517s&ab_channel=SasquatchBStudios
public class SoundMixerManager : MonoBehaviour
{
    public static SoundMixerManager Instance;

    public AudioMixer audioMixer;
    public UnityEvent onLoad;
    public List<SoundMixerParam> mixerParameterName;
    private Dictionary<SoundMixerParam, float> _dictionary = new();

    public static event Action<SoundMixerParam, float> OnLoad;

    private void Awake()
    {
        if (Instance != null)
        {
            onLoad?.Invoke();
            Destroy(gameObject);
            return;
        }
        Instance = this;
#if !UNITY_EDITOR
        Load();
#endif
    }

#if UNITY_EDITOR
    void Start() => Load();
#endif

    public void SetMixerValue(SoundMixerParam param, float val)
    {
        float requiredMixerValue = val == 0 ? -80 : Mathf.Log10(val) * 20f;
        audioMixer.SetFloat(param.ToString(), requiredMixerValue);
        PersistantData.Instance.SetData(CreateKey(param), val);
    }

    public void Load()
    {
        mixerParameterName.ForEach(param =>
        {
            if (!PersistantData.Instance.TryGetData(CreateKey(param), out float value))
            {
                Debug.Log($"Failed to get data for {param}, setting default value.");
                value = 1f;
            }
            else
            {
                Debug.Log($"Found {param} data {value}");
            }
            _dictionary.TryAdd(param, value);
            OnLoad?.Invoke(param, value);
            SetMixerValue(param, value);
        });
        onLoad?.Invoke();
    }

    private string CreateKey(SoundMixerParam param) => $"Audio_{param.ToString()}";

    public bool TryGetValue(SoundMixerParam param, out float val) => _dictionary.TryGetValue(param, out val);
}

public enum SoundMixerParam
{
    Master,
    Music,
    SFX
}