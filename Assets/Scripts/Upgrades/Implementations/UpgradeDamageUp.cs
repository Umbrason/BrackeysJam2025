using UnityEngine;

public class UpgradeDamageUp : IUpgrade
{
    public Sprite Icon => Resources.Load<Sprite>("DamageUp");
    public string Name => "Experimental Steroids";
    public string Description => $"Increases the damage by +{increase}";
    public bool Stackable => true;
     private const int increase = 5;
    public void OnApply(GameObject PlayerObject) => PlayerObject.GetComponent<PlayerStats>().DamagePerBullet.RegisterAdd(increase);
}