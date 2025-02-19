using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradesRegisterer 
{
    [RuntimeInitializeOnLoadMethod]
    void registerUpgrades()
    {
        UpgradeFireRate.Register();
    }
   
