using System.Collections;
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
    IEnumerator PickupRoutine()
    {
        yield return null;
        
    }
}