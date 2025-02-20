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
        collectionSpringConfig.AngularFrequency = 0;
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
        positionSpring.Velocity = 15 * Random.insideUnitCircle + 25 * Random.Range(-1, 1) * Vector2.Perpendicular(target.transform.position._xz() - transform.position._xz()).normalized;
        if (OnTriggerEnterSFX?.TryGetRandom(out var clip) ?? false) AudioSource.PlayClipAtPoint(clip, transform.position); //TODO: better audio playback, e.g. support audio mixing group
        while ((target.transform.position - transform.position).sqrMagnitude > 4)
        {
            collectionSpringConfig.AngularFrequency += Time.deltaTime * 10f;
            positionSpring.RestingPos = target.transform.position._xz();
            positionSpring.Step(Time.deltaTime);
            transform.position = positionSpring.Position._x0y();
            yield return null;
        }
        target.RegisterHealthEvent(HealthEvent.Heal((uint)HealAmount));
        if (OnCollectSFX?.TryGetRandom(out clip) ?? false) AudioSource.PlayClipAtPoint(clip, transform.position); //TODO: better audio playback, e.g. support audio mixing group
        Poolable.Owner.Return(Poolable);
    }
}
