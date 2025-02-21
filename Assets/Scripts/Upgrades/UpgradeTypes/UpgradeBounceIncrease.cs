using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeBounceIncrease : IUpgrade
{
	Sprite IUpgrade.Icon => null;
    string IUpgrade.Name => "BounceUp";
    string IUpgrade.Description => "Increases bounces of the bullet by + 4.";
    bool IUpgrade.Stackable => true;
    void IUpgrade.OnApply(GameObject PlayerObject) { PlayerObject.GetComponent<PlayerStats>().BulletBounces.RegisterAdd(4);}
}
