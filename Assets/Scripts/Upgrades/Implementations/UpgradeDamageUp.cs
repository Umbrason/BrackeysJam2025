using UnityEngine;

public class UpgradeDamageUp : IUpgrade
{
    public Sprite Icon => Resources.Load<Sprite>("DamageUp");
    public string Name => "Experimental Steroids";
    public string Description => "Increases the damage by +5";
    public bool Stackable => true;
    public void OnApply(GameObject PlayerObject) => PlayerObject.GetComponent<PlayerStats>().DamagePerBullet.RegisterAdd(5);
}