using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SoundVolumeDisplay : MonoBehaviour
{
    public TMP_Text textDisplay;
    public SoundMixerParam param;
    public Slider slider;

    private void OnEnable()
    {
        slider.onValueChanged.AddListener(OnSliderValueChanged);
        SoundMixerManager.OnLoad += OnLoad;

        if (SoundMixerManager.Instance.TryGetValue(param, out float val)) 
            OnLoad(param, val);
    }

    private void OnDisable()
    {
        slider.onValueChanged.AddListener(OnSliderValueChanged);
        SoundMixerManager.OnLoad -= OnLoad;
    }

    private void OnLoad(SoundMixerParam targetParam, float val)
    {
        if (targetParam != param)
            return;
        slider.SetValueWithoutNotify(val);
        textDisplay.text = ((int)(val * 100)).ToString();
    }

    private void OnSliderValueChanged(float val)
    {
        SoundMixerManager.Instance.SetMixerValue(param, val);
        textDisplay.text = ((int)(val * 100)).ToString();
    }
}
