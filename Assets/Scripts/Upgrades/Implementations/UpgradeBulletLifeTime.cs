using UnityEngine;

public class UpgradeBulletLifeTime : IUpgrade
{
    public Sprite Icon => Resources.Load<Sprite>("LifetimeUp");
    public string Name => "Bullet Life";
    public string Description => "Increases bullet life by +6s";
    public bool Stackable => true;
    public void OnApply(GameObject PlayerObject) => PlayerObject.GetComponent<PlayerStats>().BulletLifeTime.RegisterAdd(6);
}