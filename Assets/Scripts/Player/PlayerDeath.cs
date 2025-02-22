using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerDeath : MonoBehaviour
{
    public UnityEvent OnDeath;
    private Cached<HealthPool> cached_healthPool;
    private HealthPool HealthPool => cached_healthPool[this];

    void Start() => HealthPool.OnDepleted += OnHealthDepleted;
    private void OnDestroy() { if (HealthPool) HealthPool.OnDepleted -= OnHealthDepleted; }
    private void OnHealthDepleted() => OnDeath?.Invoke();
}
