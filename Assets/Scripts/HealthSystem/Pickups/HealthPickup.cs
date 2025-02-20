using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    Cached<GameObjectPoolable> cached_Poolable;
    GameObjectPoolable Poolable => cached_Poolable[this];

    Cached<DieOnTime> cached_Death;
    DieOnTime Death => cached_Death[this];

    [SerializeField] private AudioClipGroup OnCollectSFX;
    [SerializeField] private AudioClipGroup OnTriggerEnterSFX;

    void OnEnable()
    {
        Death.enabled = true;
        pickupRoutine = null;
        collectionSpringConfig.AngularFrequency = 10;
    }
    Coroutine pickupRoutine;
    void OnTriggerEnter(Collider collider)
    {
        if (pickupRoutine != null) return;
        var lovecraft = collider.GetComponentInParent<HealthPool>();
        if (lovecraft != null) pickupRoutine = StartCoroutine(PickupRoutine(lovecraft));
    }
    void Awake() => positionSpring = new(collectionSpringConfig);
    [SerializeField] private int HealAmount;
    [SerializeField] private Spring.Config collectionSpringConfig;
    private Vector2Spring positionSpring;
    IEnumerator PickupRoutine(HealthPool target)
    {
        Death.enabled = false;
        positionSpring.Position = transform.position._xz();
        var delta = (target.transform.position._xz() - transform.position._xz()).normalized;
        positionSpring.Velocity = 100 * (Random.insideUnitCircle - delta) + 200 * Random.Range(-1, 1) * Vector2.Perpendicular(delta);
        SFXPool.PlayAt(OnTriggerEnterSFX, transform.position);
        while ((target.transform.position - transform.position).sqrMagnitude > .64)
        {
            collectionSpringConfig.AngularFrequency += Time.deltaTime * 30f;
            positionSpring.RestingPos = target.transform.position._xz();
            positionSpring.Step(Time.deltaTime);
            transform.position = positionSpring.Position._x0y();
            yield return null;
        }
        target.RegisterHealthEvent(HealthEvent.Heal((uint)HealAmount));
        SFXPool.PlayAt(OnCollectSFX, transform.position);
        Poolable.Owner.Return(Poolable);
    }
}
