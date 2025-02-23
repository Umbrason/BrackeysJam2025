using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeBounceIncrease : MonoBehaviour,IUpgrade
{
	Sprite IUpgrade.Icon => Resources.Load<Sprite>("BounceUp");
    string IUpgrade.Name => "Sturdy Slinky";
    string IUpgrade.Description => "Bullets grow stronger with each bounce.";
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
