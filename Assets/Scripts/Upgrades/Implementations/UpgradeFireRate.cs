using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeFireRate : IUpgrade
{
	Sprite IUpgrade.Icon => null;
    string IUpgrade.Name => "FireRateUp";
    string IUpgrade.Description => "Increases the fire rate of the bullets by +5, but increases spread by +50.";
    bool IUpgrade.Stackable => true;
    void IUpgrade.OnApply(GameObject PlayerObject) { PlayerObject.GetComponent<PlayerStats>().Firerate.RegisterAdd(5); PlayerObject.GetComponent<PlayerStats>().SpreadDegrees.RegisterAdd(50);}
}
