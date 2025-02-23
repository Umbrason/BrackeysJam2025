using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeSpeed : IUpgrade
{
	Sprite IUpgrade.Icon => Resources.Load<Sprite>("MovementSpeedUp");
    string IUpgrade.Name => "TRIPple Boots";
    string IUpgrade.Description => $"Increases the speed of the player by {increase}.";
    bool IUpgrade.Stackable => true;
    const int increase = 4;
    void IUpgrade.OnApply(GameObject PlayerObject) { PlayerObject.GetComponent<PlayerStats>().Speed.RegisterAdd(increase);}
}
