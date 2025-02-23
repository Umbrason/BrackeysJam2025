using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeBulletSize : MonoBehaviour, IUpgrade
{
    Sprite IUpgrade.Icon => Resources.Load<Sprite>("BulletSizeUp");
    string IUpgrade.Name => "Dung Beatlets";
    string IUpgrade.Description => "Bullets grow in size over their lifetime";
    bool IUpgrade.Stackable => true;
    public static float GrowthPerSecond { get; private set; } = 0;
    void IUpgrade.OnApply(GameObject PlayerObject)
    {
        PlayerObject.AddComponent<UpgradeBulletSize>();
        GrowthPerSecond += 1f;
    }

    void OnDestroy()
    {
        GrowthPerSecond = 0;
    }

}
