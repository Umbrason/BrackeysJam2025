using UnityEngine;

public interface IUpgrade
{
    public static LootPool<IUpgrade> UpgradePool = new();
    Sprite Icon { get; }
    virtual AudioClipGroup UpgradeVoiceLine => Resources.Load<AudioClipGroup>("GenericUpgradePicked");
    string Name { get; }
    string Description { get; }
    bool Stackable { get; }
    void OnApply(GameObject PlayerObject);
    void Register() => UpgradePool.Push(this);
    void Unregister() => UpgradePool.Remove(this);
}
