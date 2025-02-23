using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScreenResolutionChanger : MonoBehaviour
{
    public TMP_Dropdown dropdown;

    private IEnumerator Start()
    {
        yield return new WaitUntil(() => ScreenModeManager.Instance.ScreenResolutions.Count > 0);
        SetOptions(ScreenModeManager.Instance.ScreenResolutions);
    }

    private void OnEnable()
    {
        ScreenModeManager.OnScreenResolutionsUpdated += SetOptions;
        ScreenModeManager.OnLoaded += RefreshDisplay;
        dropdown.onValueChanged.AddListener(UpdateScreenResolution);
        RefreshDisplay();
    }
    
    private void OnDisable()
    {
        if (ScreenModeManager.HasInstance) // refrain from creating instance when closing the application
        {
            ScreenModeManager.OnScreenResolutionsUpdated -= SetOptions;
            ScreenModeManager.OnLoaded += RefreshDisplay;
        }
        
        dropdown.onValueChanged.RemoveListener(UpdateScreenResolution);
    }

    private void UpdateScreenResolution(int flag)
    {
        ScreenModeManager.Instance.SetScreenResolution(flag);
        RefreshDisplay();
    }

    public void RefreshDisplay()
    {
        if (ScreenModeManager.Instance.ScreenResolutions.Count == 0)
            return;
        
        Vector2Int res = ScreenModeManager.Instance.ScreenResolutions[ScreenModeManager.Instance.ResolutionIndex];
        dropdown.SetValueWithoutNotify(ScreenModeManager.Instance.ResolutionIndex);
    }

    private void SetOptions(List<Vector2Int> options)
    {
        dropdown.ClearOptions();;

        List<string> datas = new ();
        options.ForEach(item => datas.Add($"{item.x}x{item.y}"));
        
        dropdown.AddOptions(datas);
        
        dropdown.SetValueWithoutNotify(ScreenModeManager.Instance.ResolutionIndex);
    }
}