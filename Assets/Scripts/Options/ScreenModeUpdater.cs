using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScreenModeUpdater : MonoBehaviour
{
    public TMP_Dropdown dropdown;
    
    private void Awake()
    {
        dropdown.ClearOptions();

        List<string> datas = new List<string>();
        for (int i = 0; i < 4; i++) 
            datas.Add(((FullScreenMode)i).ToString());

        dropdown.AddOptions(datas);
    }

    private void OnEnable()
    {
        ScreenModeManager.OnLoaded += RefreshDisplay;
        dropdown.onValueChanged.AddListener(UpdateScreenMode);
        RefreshDisplay();
    }

    

    private void OnDisable()
    {
        ScreenModeManager.OnLoaded -= RefreshDisplay;
        dropdown.onValueChanged.RemoveListener(UpdateScreenMode);
    }

    private void UpdateScreenMode(int flag)
    {
        ScreenModeManager.Instance.SetScreenMode(flag);
        RefreshDisplay();
    }

    private void RefreshDisplay()
    {
        dropdown.SetValueWithoutNotify(ScreenModeManager.Instance.ScreenModeIndex);
    }
}