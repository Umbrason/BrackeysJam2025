using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeBulletSize : IUpgrade
{
	Sprite IUpgrade.Icon => null;
    string IUpgrade.Name => "BulletSizeUp";
    string IUpgrade.Description => "Increases size of bullets by 4.";
    bool IUpgrade.Stackable => true;
    void IUpgrade.OnApply(GameObject PlayerObject) { PlayerObject.GetComponent<PlayerStats>().BulletRadius.RegisterAdd(4);}
}
