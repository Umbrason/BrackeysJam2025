using UnityEngine;

public class UpgradeProjectileCount : IUpgrade
{
    public Sprite Icon => null;
    public string Name => "Projectile Count Up";
    public string Description => "Increases Projectile by +1";
    public bool Stackable => true;
    public void OnApply(GameObject PlayerObject) => PlayerObject.GetComponent<PlayerStats>().BulletsPerShot.RegisterAdd(4);
}