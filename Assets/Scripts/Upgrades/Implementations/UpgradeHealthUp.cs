using UnityEngine;

public class UpgradeHealthUp : IUpgrade
{
    public Sprite Icon => null;
    public string Name => "Health Up";
    public string Description => "Increases health by +50%, decreases speed by -25%";
    public bool Stackable => true;
    public void OnApply(GameObject PlayerObject)
    {
        PlayerObject.GetComponent<PlayerStats>().MaxHealth.RegisterMultiply(1.5f);
        PlayerObject.GetComponent<PlayerStats>().Speed.RegisterMultiply(0.75f);
    }
}