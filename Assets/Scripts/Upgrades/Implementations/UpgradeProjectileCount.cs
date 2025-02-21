using UnityEngine;

public class UpgradeProjectileCount : IUpgrade
{
    public Sprite Icon => null;
    public string Name => "Projectile Count Up";
    public string Description => $"Increases Projectile by +{increase}";
    public bool Stackable => true;
    private const int increase = 4;
    public void OnApply(GameObject PlayerObject) => PlayerObject.GetComponent<PlayerStats>().BulletsPerShot.RegisterAdd(increase);
}