using UnityEngine;

public class UpgradeBulletLifeTime : IUpgrade
{
    public Sprite Icon => null;
    public string Name => "Bullet Life";
    public string Description => "Increases bullet life by +3s";
    public bool Stackable => true;
    public void OnApply(GameObject PlayerObject) => PlayerObject.GetComponent<PlayerStats>().BulletLifeTime.RegisterAdd(3);
}