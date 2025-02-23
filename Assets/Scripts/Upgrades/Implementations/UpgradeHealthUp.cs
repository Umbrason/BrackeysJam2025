using UnityEngine;

public class UpgradeHealthUp : IUpgrade
{
    public Sprite Icon => Resources.Load<Sprite>("HealthUpSpeedDown");
    public string Name => "Turtle Shell";
    public string Description => "Increases health by +50%, decreases speed by -25%";
    public bool Stackable => true;
    public void OnApply(GameObject PlayerObject)
    {
        PlayerObject.GetComponent<PlayerStats>().MaxHealth.RegisterMultiply(1.5f);
        PlayerObject.GetComponent<PlayerStats>().Speed.RegisterMultiply(0.75f);
    }
}