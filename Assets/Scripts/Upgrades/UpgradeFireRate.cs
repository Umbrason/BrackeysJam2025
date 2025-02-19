using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeFireRate : IUpgrade
{
	Sprite IUpgrade.Icon => null;
    string IUpgrade.Description => "Test";
    bool IUpgrade.Stackable => true;
    void IUpgrade.OnApply(GameObject PlayerObject) { PlayerObject.GetComponent<PlayerStats>().Firerate;}
}
