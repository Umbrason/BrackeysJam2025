using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradesRegisterer 
{
    public IUpgrade[] UpgradeList;

    [RuntimeInitializeOnLoadMethod]
    void registerUpgrades()
    {
        for (int i = 0; i < UpgradeList.Length; i++)
        {
            UpgradeList[i].Register();

        }
    }
}
