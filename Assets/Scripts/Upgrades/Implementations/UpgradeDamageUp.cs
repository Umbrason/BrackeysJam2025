using UnityEngine;

public class UpgradeDamageUp : IUpgrade
{
    public Sprite Icon => null;
    public string Name => "Damage Up";
    public string Description => $"Increases the damage by +{increase}";
    public bool Stackable => true;
     private const int increase = 10;
    public void OnApply(GameObject PlayerObject) => PlayerObject.GetComponent<PlayerStats>().DamagePerBullet.RegisterAdd(10);
}