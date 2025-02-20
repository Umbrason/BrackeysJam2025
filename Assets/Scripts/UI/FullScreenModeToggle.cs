using UnityEngine;
using UnityEngine.UI;

public class FullScreenModeToggle : MonoBehaviour
{
    public Toggle toggle;

    private void OnEnable()
    {
        toggle.onValueChanged.AddListener(UpdateFullScreenMode);
        toggle.SetIsOnWithoutNotify(ScreenModeManager.Instance.IsFullScreen);
    }

    private void OnDisable() => toggle.onValueChanged.RemoveListener(UpdateFullScreenMode);

    private void UpdateFullScreenMode(bool flag) => ScreenModeManager.Instance.SetFullScreenMode(flag);
}