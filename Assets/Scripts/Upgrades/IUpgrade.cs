using UnityEngine;

public interface IUpgrade
{
    public static LootPool<IUpgrade> UpgradePool;
    Sprite Icon { get; }
    string Name { get; }
    string Description { get; }
    bool Stackable { get; }
    void OnApply(GameObject PlayerObject);
    void Register() => UpgradePool.Add(this);
    void Unregister() => UpgradePool.Remove(this);
}
