using System.Collections;
using System.Linq;
using UnityEngine;

public class UpgradePickup : MonoBehaviour
{
    void OnTriggerEnter() => TriggerPickUp();
    void TriggerPickUp()
    {
        if (pickingUp) return;
        pickingUp = true;
        StartCoroutine(PickupRoutine());
    }
    bool pickingUp = false;
    const int UpgradeOptionCount = 3;
    IEnumerator PickupRoutine()
    {
        yield return null;
        var options = Enumerable.Range(0, UpgradeOptionCount).Select(_ => IUpgrade.UpgradePool.Pull()).ToArray();
        UpgradeSelection.Show(options);
    }
}