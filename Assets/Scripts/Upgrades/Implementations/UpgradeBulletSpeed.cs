using UnityEngine;

public class UpgradeBulletSpeed : IUpgrade
{
    public Sprite Icon => Resources.Load<Sprite>("ProjectileSpeedUp");
    public string Name => "Energetic Bullets";
    public string Description => "Increases bullets speed by +1.";
    public bool Stackable => true;
    public void OnApply(GameObject PlayerObject) => PlayerObject.GetComponent<PlayerStats>().BulletVelocity.RegisterAdd(1);
}