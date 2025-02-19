using UnityEngine;

public interface IUpgrade
{
    public static LootPool<IUpgrade> UpgradePool = new();
    Sprite Icon { get; }
    string Name { get; }
    string Description { get; }
    bool Stackable { get; }
    void OnApply(GameObject PlayerObject);
    protected void Register() => UpgradePool.Push(this);
    protected void Unregister() => UpgradePool.Remove(this);
}