using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenModeManager : MonoBehaviour, ISaveLoad
{
    public static ScreenModeManager Instance { get; private set; }
    
    public bool IsFullScreen {get; private set; }
    public int ResolutionIndex {get; private set; }
    public int ScreenModeIndex {get; private set; }

    public event Action<List<Vector2Int>> OnScreenResolutionsUpdated; 
    public List<Vector2Int> ScreenResolutions { get; private set; } = new();
    
    public string KeyFullScreen => $"Screen_FullScreen";
    public string KeyScreenMode => $"Screen_Mode";
    public string KeyScreenResolution => $"Screen_Resolution";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy() => Save();

    private IEnumerator Start()
    {
        SetupScreenResolutions();
        yield return new WaitForSeconds(0.1f);
        Load();
    }

    public void SetFullScreenMode(bool flag)
    {
        IsFullScreen = flag;
        Screen.fullScreen = IsFullScreen;
    }

    public void SetScreenMode(int flag)
    {
        FullScreenMode mode = (FullScreenMode)flag;
        ScreenModeIndex = flag;
        Screen.SetResolution(ScreenResolutions[ResolutionIndex].x, ScreenResolutions[ResolutionIndex].y, mode);
    }

    public void SetScreenResolution(int flag)
    {
        if (flag >= ScreenResolutions.Count)
            return;

        ResolutionIndex = flag;
        SetScreenMode(ScreenModeIndex);
    }

    public void SetupScreenResolutions()
    {
        ScreenResolutions.Clear();

        float multiplier = 1;
        int width = Screen.currentResolution.width;
        int height = Screen.currentResolution.height;
        
        for (int i = 0; i <= 5; i++)
        {
            ScreenResolutions.Add(new Vector2Int((int)(width * multiplier), (int)(height * multiplier)));
            multiplier -= 0.0666f;
        }
        
        OnScreenResolutionsUpdated?.Invoke(ScreenResolutions);
    }

    public void Load()
    {
        IsFullScreen = PlayerPrefs.GetInt(KeyFullScreen, 1) == 1;
        ScreenModeIndex = PlayerPrefs.GetInt(KeyScreenMode, 0);
        ResolutionIndex = PlayerPrefs.GetInt(KeyScreenResolution, ResolutionIndex);
        
        SetFullScreenMode(IsFullScreen);
        SetScreenResolution(ResolutionIndex);
    }

    public void Save()
    {
        PlayerPrefs.SetInt(KeyFullScreen, IsFullScreen ? 1 : 0);
        PlayerPrefs.SetInt(KeyScreenMode, ScreenModeIndex);
        PlayerPrefs.SetInt(KeyScreenResolution, ResolutionIndex);
        PlayerPrefs.Save();
    }
}