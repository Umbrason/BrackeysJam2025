using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeSpeed : IUpgrade
{
	Sprite IUpgrade.Icon => null;
    string IUpgrade.Name => "SpeedUp";
    string IUpgrade.Description => "Increases speed of the player by 4.";
    bool IUpgrade.Stackable => true;
    void IUpgrade.OnApply(GameObject PlayerObject) { PlayerObject.GetComponent<PlayerStats>().Speed.RegisterAdd(4);}
}
