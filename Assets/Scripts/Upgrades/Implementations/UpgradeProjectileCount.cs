using UnityEngine;

public class UpgradeProjectileCount : IUpgrade
{
    public Sprite Icon => Resources.Load<Sprite>("ProjectileCountUp");
    public string Name => "Viral Replicator";
    public string Description => $"Increases Projectile count by +{increase}";
    public bool Stackable => true;
    private const int increase = 1;
    private const int otherIncrease = 2;
    public void OnApply(GameObject PlayerObject) 
    {
        PlayerObject.GetComponent<PlayerStats>().BulletsPerShot.RegisterAdd(increase);
        PlayerObject.GetComponent<PlayerStats>().SpreadDegrees.RegisterAdd(otherIncrease);
}
}

    