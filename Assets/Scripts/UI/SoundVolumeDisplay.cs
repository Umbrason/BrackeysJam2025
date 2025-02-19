using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SoundVolumeDisplay : MonoBehaviour
{
    public TMP_Text textDisplay;
    public Slider slider;

    private void OnEnable() => slider.onValueChanged.AddListener(OnSliderValueChanged);
    private void OnDisable() => slider.onValueChanged.AddListener(OnSliderValueChanged);

    public void SetSliderValue(float val)
    {
        slider.SetValueWithoutNotify(val);
        OnSliderValueChanged(val);
    }
    
    private void OnSliderValueChanged(float val) => textDisplay.text = ((int)(val * 100)).ToString();
}
