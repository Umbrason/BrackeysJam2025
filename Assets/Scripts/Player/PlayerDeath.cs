using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerDeath : MonoBehaviour
{
    public UnityEvent OnDeath;
    
    private HealthPool healthPool;


    private void MakePlayerDead()
    {
        OnDeath?.Invoke();
    }

    private void Awake()
    {
        healthPool = GetComponent<HealthPool>();
    }

    // Start is called before the first frame update
    void Start()
    {
        healthPool.OnDepleted += MakePlayerDead; 
    }

    private void OnDestroy()
    {
        healthPool.OnDepleted -= MakePlayerDead;
    }
}
