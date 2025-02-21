using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreOverTime : MonoBehaviour
{
    [SerializeField] float scoreInterval = 1f;
    float startTime;

    void Awake() => startTime = Time.time;

    void FixedUpdate()
    {
        TransientScoring.SetTimeAlive(Mathf.FloorToInt((Time.time - startTime) / scoreInterval));
    }
}
