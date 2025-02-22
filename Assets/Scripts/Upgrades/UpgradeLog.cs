using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeLog : MonoBehaviour
{
    public static readonly Dictionary<IUpgrade, int> counts = new();
    public static void Log(IUpgrade upgrade)
    {
        if (!counts.ContainsKey(upgrade))
            counts[upgrade] = 0;
        counts[upgrade]++;
    }

    void OnDisable() => counts.Clear();
}
