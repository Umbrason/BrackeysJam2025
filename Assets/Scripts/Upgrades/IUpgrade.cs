using UnityEngine;

public interface IUpgrade
{
    Sprite Icon { get; }
    string Description { get; }
    void OnApply(GameObject PlayerObject);
}
