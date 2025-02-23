using UnityEngine;

public class UpgradeBulletLifeTime : MonoBehaviour, IUpgrade
{
    public Sprite Icon => Resources.Load<Sprite>("LifetimeUp");
    public string Name => "The Singed Dragon of RAt";
    public string Description => $"Increases bullet lifetime by +{increase}s";
    public bool Stackable => false;
    private const int increase = 10;

    public static bool IsActive { get; set; }

    public void OnApply(GameObject PlayerObject)
    {
        PlayerObject.GetComponent<PlayerStats>().BulletLifeTime.RegisterAdd(increase);
        PlayerObject.AddComponent<UpgradeBulletLifeTime>();
        IsActive = true;
    }

    public void OnDestroy() => IsActive = false;
}