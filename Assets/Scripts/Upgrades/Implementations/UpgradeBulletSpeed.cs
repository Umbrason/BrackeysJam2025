using UnityEngine;

public class UpgradeBulletSpeed : IUpgrade
{
    public Sprite Icon => Resources.Load<Sprite>("ProjectileSpeedUp");
    public string Name => "Energetic Bullets";
    public string Description => $"Increases bullets speed by +{increase}.";
    public bool Stackable => true;
     private const int increase = 10;
    public void OnApply(GameObject PlayerObject) => PlayerObject.GetComponent<PlayerStats>().BulletVelocity.RegisterAdd(increase);
}