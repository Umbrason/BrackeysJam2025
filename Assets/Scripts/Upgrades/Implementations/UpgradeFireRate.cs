using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeFireRate : IUpgrade
{
	Sprite IUpgrade.Icon => Resources.Load<Sprite>("FirerateUp");
    string IUpgrade.Name => "Optimized Magazine";
    string IUpgrade.Description => "Increases the fire rate of the bullets by +2, but increases spread by +20 degrees.";
    AudioClipGroup IUpgrade.UpgradeVoiceLine => Resources.Load<AudioClipGroup>("FireRateUpgradePicked");
    bool IUpgrade.Stackable => true;
    void IUpgrade.OnApply(GameObject PlayerObject) { PlayerObject.GetComponent<PlayerStats>().Firerate.RegisterAdd(2); PlayerObject.GetComponent<PlayerStats>().SpreadDegrees.RegisterAdd(20);}
}
