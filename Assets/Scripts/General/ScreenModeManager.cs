using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenModeManager : MonoBehaviour, ISaveLoad
{
    private static ScreenModeManager _instance;
    public static bool HasInstance => _instance != null;
    public static ScreenModeManager Instance
    {
        get
        {
            if (_instance == null)
            {
                var obj = new GameObject(nameof(ScreenModeManager));
                _instance = obj.AddComponent<ScreenModeManager>();
                DontDestroyOnLoad(obj);
            }
            
            return _instance;
        }
    }
    
    public bool IsFullScreen {get; private set; }
    public int ResolutionIndex {get; private set; }
    public int ScreenModeIndex {get; private set; }

    public static event Action OnLoaded;

    public static event Action<List<Vector2Int>> OnScreenResolutionsUpdated; 
    public List<Vector2Int> ScreenResolutions { get; private set; } = new();
    
    public string KeyFullScreen => $"Screen_FullScreen";
    public string KeyScreenMode => $"Screen_Mode";
    public string KeyScreenResolution => $"Screen_Resolution";

    private IEnumerator Start()
    {
        SetupScreenResolutions();
        yield return new WaitForSeconds(0.1f);
        Load();
    }

    public void SetFullScreenMode(bool flag)
    {
        IsFullScreen = flag;
        if (PersistantData.Instance != null)
            PersistantData.Instance.SetData(KeyFullScreen, IsFullScreen ? 1 : 0);
        Screen.fullScreen = IsFullScreen;
    }

    public void SetScreenMode(int flag)
    {
        FullScreenMode mode = (FullScreenMode)flag;
        ScreenModeIndex = flag;
        if (PersistantData.Instance != null)
            PersistantData.Instance.SetData(KeyScreenMode, ScreenModeIndex);
        Screen.SetResolution(ScreenResolutions[ResolutionIndex].x, ScreenResolutions[ResolutionIndex].y, mode);
    }

    public void SetScreenResolution(int flag)
    {
        if (flag >= ScreenResolutions.Count)
            return;
        ResolutionIndex = flag;
        if (PersistantData.Instance != null)
            PersistantData.Instance.SetData(KeyScreenResolution, ResolutionIndex);
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
        if(!PersistantData.Instance.TryGetData(KeyFullScreen, out int valueFullScreen)) 
            valueFullScreen = 1;
        IsFullScreen = valueFullScreen == 1;

        if(!PersistantData.Instance.TryGetData(KeyScreenMode, out int valueScreenMode)) 
            valueScreenMode = 0;
        ScreenModeIndex = valueScreenMode;

        if(!PersistantData.Instance.TryGetData(KeyScreenResolution, out int resolutionIndex))
            resolutionIndex = ResolutionIndex;
        ResolutionIndex = resolutionIndex;
        
        SetFullScreenMode(IsFullScreen);
        SetScreenResolution(ResolutionIndex);
        
        OnLoaded?.Invoke();
    }

    public void Save()
    {
        // Does nothing
    }
}