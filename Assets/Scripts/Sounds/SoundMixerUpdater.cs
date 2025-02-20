using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;
using UnityEngine.UI;

// https://www.youtube.com/watch?v=DU7cgVsU2rM&t=517s&ab_channel=SasquatchBStudios
public class SoundMixerUpdater : MonoBehaviour, ISaveLoad
{
    public AudioMixer audioMixer;
    public string mixerParameterName;
    public Slider slider;

    public UnityEvent<float> onLoad;

    public string Key => $"Audio_{mixerParameterName}";

    private void OnDestroy() => Save();

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(0.1f);
        Load();
    }

    private void OnEnable()
    {
        if (slider != null)
            slider.onValueChanged.AddListener(SetMixerValue);
    }

    private void OnDisable()
    {
        if (slider != null)
            slider.onValueChanged.RemoveListener(SetMixerValue);
    }

    public void SetMixerValue(float val) => audioMixer.SetFloat(mixerParameterName, val == 0 ? -80 : Mathf.Log10(val) * 20f);

    public void Save()
    {
        PlayerPrefs.SetFloat(Key, slider.value);
        PlayerPrefs.Save();
    }

    public void Load()
    {
        float value = PlayerPrefs.GetFloat(Key, 1);
        onLoad?.Invoke(value);
        SetMixerValue(value);
    }
}