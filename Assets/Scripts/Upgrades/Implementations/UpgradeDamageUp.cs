using UnityEngine;

public class UpgradeDamageUp : IUpgrade
{
    public Sprite Icon => null;
    public string Name => "Damage Up";
    public string Description => "Increases the damage by +10";
    public bool Stackable => true;
    public void OnApply(GameObject PlayerObject) => PlayerObject.GetComponent<PlayerStats>().DamagePerBullet.RegisterAdd(10);
}