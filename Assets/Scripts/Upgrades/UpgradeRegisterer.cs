using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UpgradesRegisterer
{
    public static IUpgrade[] UpgradeList = {
        new UpgradeFireRate(),
        new UpgradeBounceIncrease(),
        new UpgradeBulletSize(),
        new UpgradeSpeed(),
        new AlienEye()
    };

    [RuntimeInitializeOnLoadMethod]
    public static void registerUpgrades()
    {
        for (int i = 0; i < UpgradeList.Length; i++)
        {
            UpgradeList[i].Register();

        }
    }
}
