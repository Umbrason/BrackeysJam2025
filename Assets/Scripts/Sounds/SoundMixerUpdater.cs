using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;
using UnityEngine.UI;

// https://www.youtube.com/watch?v=DU7cgVsU2rM&t=517s&ab_channel=SasquatchBStudios
public class SoundMixerUpdater : MonoBehaviour
{
    public AudioMixer audioMixer;
    public string mixerParameterName;
    public Slider slider;

    public UnityEvent<float> onLoad;

    public string Key => $"Audio_{mixerParameterName}";

    private void Awake() => Load();
    private void OnDestroy() => Save();

    private void OnEnable() => slider.onValueChanged.AddListener(SetMixerValue);
    private void OnDisable() => slider.onValueChanged.RemoveListener(SetMixerValue);
    
    public void SetMixerValue(float val) => audioMixer.SetFloat(mixerParameterName, val == 0 ? -80 : Mathf.Log10(val) * 20f);

    public void Save()
    {
        Debug.Log($"Pref Save {Key} {slider.value}");
        PlayerPrefs.SetFloat(Key, slider.value);
        PlayerPrefs.Save();
    }

    public void Load()
    {
        float value = PlayerPrefs.GetFloat(Key, 1);
        Debug.Log($"Pref Load {Key} {value}");
        onLoad?.Invoke(value);
        SetMixerValue(value);
    }
}