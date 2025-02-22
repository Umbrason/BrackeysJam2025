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

    [SerializeField] UpgradeLogEntry template;
    void OnEnable()
    {
        while (transform.childCount > 0)
            Destroy(transform.GetChild(0));
        foreach (var (upgrade, count) in counts)
        {
            var instance = Instantiate(template, transform);
            instance.Upgrade = upgrade;
            instance.Count = count;
        }
    }

    void OnDisable() => counts.Clear();
}
