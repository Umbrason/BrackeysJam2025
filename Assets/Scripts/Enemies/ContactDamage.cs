using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactDamage : MonoBehaviour
{
    [SerializeField] private float damageInterval;
    [SerializeField] private int damageAmount;
    int playerLayer => LayerMask.NameToLayer("Player");
    private float lastDamageTime;

    void OnCollisionEnter(Collision other) => TryDamage(other.collider.gameObject);
    void OnCollisionStay(Collision other) => TryDamage(other.collider.gameObject);

    void TryDamage(GameObject target)
    {
        if (Time.time - lastDamageTime <= damageInterval) return;
        lastDamageTime = Time.time;
        if (target.layer != playerLayer) return;
        var hitbox = target.GetComponentInParent<Hitbox>();
        hitbox.RegisterDamageEvent(HealthEvent.Damage((uint)damageAmount, gameObject));
    }

}
