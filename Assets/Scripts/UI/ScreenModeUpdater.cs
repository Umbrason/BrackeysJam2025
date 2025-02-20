using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScreenModeUpdater : MonoBehaviour
{
    public TMP_Dropdown dropdown;
    public TMP_Text text;

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
        dropdown.onValueChanged.AddListener(UpdateScreenMode);
        RefreshDisplay();
    }

    private void OnDisable() => dropdown.onValueChanged.RemoveListener(UpdateScreenMode);

    private void UpdateScreenMode(int flag)
    {
        ScreenModeManager.Instance.SetScreenMode(flag);
        RefreshDisplay();
    }

    private void RefreshDisplay()
    {
        text.text = ((FullScreenMode)ScreenModeManager.Instance.ScreenModeIndex).ToString();
        dropdown.SetValueWithoutNotify(ScreenModeManager.Instance.ScreenModeIndex);
    }
}