using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeFireRate : IUpgrade
{
	Sprite IUpgrade.Icon => Resources.Load<Sprite>("FirerateUp");
    string IUpgrade.Name => "Optimized Magazine";
    string IUpgrade.Description => "Increases the fire rate of the bullets by +5, but increases spread by +50.";
    AudioClipGroup IUpgrade.UpgradeVoiceLine => Resources.Load<AudioClipGroup>("FireRateUpgradePicked");
    bool IUpgrade.Stackable => true;
    void IUpgrade.OnApply(GameObject PlayerObject) { PlayerObject.GetComponent<PlayerStats>().Firerate.RegisterAdd(5); PlayerObject.GetComponent<PlayerStats>().SpreadDegrees.RegisterAdd(50);}
}
