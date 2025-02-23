using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienEye : MonoBehaviour, IUpgrade
{
    public Sprite Icon => Resources.Load<Sprite>($"AlienEye");
    public string Name => "Alien Eye";
    public string Description => "Lets you see more...";
    AudioClipGroup IUpgrade.UpgradeVoiceLine => Resources.Load<AudioClipGroup>("FontUpgradePicked");
    public bool Stackable => false;
    public static bool IsActive { get; private set; }
    public static event Action<bool> ActiveChanged;

    public void OnApply(GameObject PlayerObject)
    {
        PlayerObject.AddComponent<AlienEye>();
        IsActive = true;
        ActiveChanged?.Invoke(IsActive);
    }

    void OnDestroy()
    {
        IsActive = false;
        ActiveChanged?.Invoke(IsActive);
    }
}
