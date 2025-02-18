using System.Collections.Generic;
using UnityEngine;

public interface IUpgrade
{
    public static LootPool<IUpgrade> UpgradePool;
    Sprite Icon { get; }
    string Description { get; }
    bool Stackable { get; }
    void OnApply(GameObject PlayerObject);
    protected void Register() => UpgradePool.Add(this);
    protected void Unregister() => UpgradePool.Remove(this);
}
