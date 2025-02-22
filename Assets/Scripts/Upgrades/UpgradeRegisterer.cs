using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public static class UpgradesRegisterer
{
    [RuntimeInitializeOnLoadMethod]
    public static void registerUpgrades()
    {
        var upgradeTypes = Assembly.GetAssembly(typeof(IUpgrade)).GetTypes().Where(type => typeof(IUpgrade).IsAssignableFrom(type));
        foreach (var upgrade in upgradeTypes)
        {
            if (upgrade.GetCustomAttribute<DontAutoRegisterUpgradeAttribute>() != null) continue;
            if (upgrade.GetConstructor(Type.EmptyTypes) != null)
            {
                var instance = (IUpgrade)Activator.CreateInstance(upgrade);
                instance.Register();
            }
        }
    }
}
