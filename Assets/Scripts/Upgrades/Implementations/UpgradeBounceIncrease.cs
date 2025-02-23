using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeBounceIncrease : MonoBehaviour,IUpgrade
{
	Sprite IUpgrade.Icon => null;
    string IUpgrade.Name => "BounceUp";
    string IUpgrade.Description => "Increases bounces of the bullet by + 4.";
    bool IUpgrade.Stackable => true;
   public static bool IsActive { get; private set; }
    void IUpgrade.OnApply(GameObject PlayerObject) 
    { 
        PlayerObject.AddComponent<UpgradeBounceIncrease>();
        PlayerObject.GetComponent<PlayerStats>().BulletBounces.RegisterAdd(4);
        IsActive = true;
        //bullet grows in size and damage each bounce       
        
    }
}
