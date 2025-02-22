using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PersistantData : MonoBehaviour
{
    private static PersistantData instance;
    public static event Action<string> SettingChanged;
    public static PersistantData Instance
    {
        get
        {
            if(instance == null)
            {
                GameObject go = new GameObject("Persistant data");
                instance = go.AddComponent<PersistantData>();
                DontDestroyOnLoad(go);
            }

            return instance;
        }
    }

    public void SetData(string key, int value)
    {
        PlayerPrefs.SetInt(key, value);
        SettingChanged?.Invoke(key);
    }

    public void SetData(string key, float value)
    {
        PlayerPrefs.SetFloat(key, value);
        SettingChanged?.Invoke(key);
    }

    public void SetData(string key, string value)
    {
        PlayerPrefs.SetString(key, value);
        SettingChanged?.Invoke(key);
    }

    public bool TryGetData(string key, out int value)
    {
        if(PlayerPrefs.HasKey(key))
        {
            value = PlayerPrefs.GetInt(key);
            return true;
        }

        value = -1;
        return false;
    }

    public bool TryGetData(string key, out float value)
    {
        if (PlayerPrefs.HasKey(key))
        {
            value = PlayerPrefs.GetFloat(key);
            return true;
        }

        value = -1.0f;
        return false;
    }

    public bool TryGetData(string key, out string value)
    {
        if (PlayerPrefs.HasKey(key))
        {
            value = PlayerPrefs.GetString(key);
            return true;
        }

        value = "";
        return false;
    }

    public bool IsDataExists(string key)
    {
        return PlayerPrefs.HasKey(key);
    }

    public void Save()
    {
        PlayerPrefs.Save();
    }

    private void CheckForDuplicateInstances(Scene oldScene, Scene newScene)
    {
        if(instance == null)
        {
            return;
        }

        var allInstances = FindObjectsByType<PersistantData>(FindObjectsInactive.Include,
                                                                     FindObjectsSortMode.InstanceID);

        for (int i = 0; i < allInstances.Length; i++)
        {
            var current = allInstances[i];
            if (current.gameObject.GetInstanceID() == instance.gameObject.GetInstanceID())
            {
                continue;
            }
            else
            {
                Destroy(current.gameObject);
            }
        }
    }

    private void Start()
    {
        SceneManager.activeSceneChanged += CheckForDuplicateInstances;
    }

    private void OnApplicationFocus(bool focus)
    {
        if(!focus)
        {
            Save();
        }
    }

    private void OnDestroy()
    {
        Save();
        SceneManager.activeSceneChanged -= CheckForDuplicateInstances;
    }
}
