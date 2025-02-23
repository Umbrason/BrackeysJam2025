using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LowHealthIndicator : MonoBehaviour
{
    [SerializeField] private HealthPool pool;

    Cached<Image> cached;
    Image Image => cached[this];

    void Start()
    {
        pool.OnModified += OnModified;
    }

    void OnDestroy() { if (pool) pool.OnModified -= OnModified; }

    float lowThreshold = .3f;
    void OnModified(int amount)
    {
        Image.enabled = pool.Current / (float)pool.Size < lowThreshold;
    }
}
