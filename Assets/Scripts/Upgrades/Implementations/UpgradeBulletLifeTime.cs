using UnityEngine;

public class UpgradeBulletLifeTime : IUpgrade
{
    public Sprite Icon => Resources.Load<Sprite>("LifetimeUp");
    public string Name => "Bullet Life";
    public string Description => $"Increases bullet life by +{increase}s";
    public bool Stackable => true;
    private const int increase = 10;

    public void OnApply(GameObject PlayerObject) => PlayerObject.GetComponent<PlayerStats>().BulletLifeTime.RegisterAdd(increase);
}