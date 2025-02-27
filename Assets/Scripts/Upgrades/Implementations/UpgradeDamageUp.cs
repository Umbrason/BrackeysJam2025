using UnityEngine;

public class UpgradeDamageUp : IUpgrade
{
    public Sprite Icon => Resources.Load<Sprite>("DamageUp");
    public string Name => "Experimental Steroids";
    public string Description => $"Increases the damage by +{Mathf.RoundToInt((increase - 1) * 100)}%";
    public bool Stackable => true;
     private const float increase = 1.3f;
    public void OnApply(GameObject PlayerObject) => PlayerObject.GetComponent<PlayerStats>().DamagePerBullet.RegisterMultiply(increase);
}