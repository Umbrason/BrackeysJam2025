using UnityEngine;
using UnityEngine.UI;

public class FullScreenModeToggle : MonoBehaviour
{
    public Toggle toggle;

    private void OnEnable()
    {
        ScreenModeManager.OnLoaded += OnLoad;
        toggle.onValueChanged.AddListener(UpdateFullScreenMode);
        OnLoad();
    }

    private void OnDisable()
    {
        ScreenModeManager.OnLoaded -= OnLoad;
        toggle.onValueChanged.RemoveListener(UpdateFullScreenMode);
    }

    private void UpdateFullScreenMode(bool flag) => ScreenModeManager.Instance.SetFullScreenMode(flag);
    
    private void OnLoad() => toggle.SetIsOnWithoutNotify(ScreenModeManager.Instance.IsFullScreen);
}