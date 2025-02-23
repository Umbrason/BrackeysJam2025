using UnityEngine;

public class UpgradeBulletSpeed : IUpgrade
{
    public Sprite Icon => null;
    public string Name => "Bullet Speed Up";
    public string Description => "Increases bullets speed by +10.";
    public bool Stackable => true;
    public void OnApply(GameObject PlayerObject) => PlayerObject.GetComponent<PlayerStats>().BulletVelocity.RegisterAdd(10);
}