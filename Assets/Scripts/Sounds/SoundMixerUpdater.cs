using System;
using System.Collections;
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

    //private void OnDestroy() => Save();

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(0.1f);
        Load();
    }

    private void OnEnable() => slider.onValueChanged.AddListener(SetMixerValue);
    private void OnDisable() => slider.onValueChanged.RemoveListener(SetMixerValue);

    public void SetMixerValue(float val)
    {
        float requiredMixerValue = val == 0 ? -80 : Mathf.Log10(val) * 20f;
        audioMixer.SetFloat(mixerParameterName, requiredMixerValue);
        PersistantData.Instance.SetData(Key, val);
    }
    /*public void Save()
    {
        Debug.Log($"Pref Save {Key} {slider.value}");
        PlayerPrefs.SetFloat(Key, slider.value);
        PlayerPrefs.Save();
    }*/

    public void Load()
    {
        float value = 0.0f;
        if (!PersistantData.Instance.TryGetData(Key, out value))
        {
            Debug.Log("Failed to get data for sound, setting default value.");
            //value = 1.0f;
        }
        else
        {
            Debug.Log("Found sound data");
        }
        Debug.Log($"Pref Load {Key} {value}");
        onLoad?.Invoke(value);
        SetMixerValue(value);
    }
}