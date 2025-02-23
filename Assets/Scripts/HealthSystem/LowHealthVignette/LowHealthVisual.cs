using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LowHealthVisual : MonoBehaviour
{
    [Range(0f, 1f)]
    [SerializeField] private float threshold = .4f;
    [SerializeField] private HealthPool playerHealth;

    Cached<CanvasGroup> cached_CG;
    CanvasGroup CG => cached_CG[this];

    BaseSpring OpacitySpring = new(new(20, 1));

    void Start()
    {
        if (!playerHealth) return;
        playerHealth.OnModified += OnHealthChanged;
    }

    void OnDestroy()
    {
        if (!playerHealth) return;
        playerHealth.OnModified -= OnHealthChanged;
    }

    void OnHealthChanged(int delta)
    {
        OpacitySpring.RestingPos = Mathf.Clamp01(1 - playerHealth.Current / (float)playerHealth.Size / threshold);
    }

    private void Update()
    {
        OpacitySpring.Step(Time.deltaTime);
        CG.alpha = OpacitySpring.Position;
    }

}
