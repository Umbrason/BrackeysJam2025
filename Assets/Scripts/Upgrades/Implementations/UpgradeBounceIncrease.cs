using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeBounceIncrease : IUpgrade
{
	Sprite IUpgrade.Icon => Resources.Load<Sprite>("BounceUp");
    string IUpgrade.Name => "Sturdy Slinky";
    string IUpgrade.Description => "Bullets grow stronger with each bounce.";
    bool IUpgrade.Stackable => true;
    void IUpgrade.OnApply(GameObject PlayerObject) { PlayerObject.GetComponent<PlayerStats>().BulletBounces.RegisterAdd(4);}
}
