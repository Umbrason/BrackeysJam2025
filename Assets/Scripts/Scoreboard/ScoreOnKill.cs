using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreOnKill : MonoBehaviour
{
    Cached<HealthPool> cached_healthpool;
    HealthPool healthpool => cached_healthpool[this];


    void Awake()
    {
        healthpool.OnDepleted += () => TransientScoring.AddEnemyKillScore(healthpool.Size);
    }
}
