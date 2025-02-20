using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    [Header("Scene Refs")]
    [SerializeField] private HealthPool targetHealthPool;
    [Header("Internal Refs")]
    [SerializeField] private Image FillMask;
    [SerializeField] private Graphic FillImage;
    [SerializeField] private Graphic BackgroundImage;
    [Header("Visual Config")]
    [SerializeField] private Gradient FillColorGradient;
    [SerializeField] private Gradient BackgroundColorGradient;
    [SerializeField] private Spring.Config fillSpringConfig;
    private BaseSpring FillSpring;
    [SerializeField] private Spring.Config fillColorSpringConfig;
    private BaseSpring FillColorSpring;

    [SerializeField] private Spring.Config shakeSpringConfig;
    private RotationSpring ShakeSpring;
    [SerializeField] private float MinShakeStrength = 10;
    [SerializeField] private float MaxShakeStrength = 20;

    [Header("Visibility Settings")]
    [SerializeField] private float AlwaysVisibleBelowThreshold = .3f;
    [SerializeField] private float FadeOutDuration = .2f;
    [SerializeField] private float FadeOutDelay = 1f;
    private float lastHealthEventTime;

    Cached<CanvasGroup> cached_canvasGroup;
    CanvasGroup CanvasGroup => cached_canvasGroup[this];

    void Awake()
    {
        FillSpring = new(fillSpringConfig)
        {
            Position = 1,
            RestingPos = 1
        };
        FillSpring.OnSpringUpdated += f => FillMask.fillAmount = f;
        FillColorSpring = new(fillColorSpringConfig);
        FillColorSpring.OnSpringUpdated += f =>
        {
            FillImage.color = FillColorGradient.Evaluate(f);
            BackgroundImage.color = BackgroundColorGradient.Evaluate(f);
        };
        ShakeSpring = new(shakeSpringConfig)
        {
            Position = 0,
            RestingPos = 0,
        };
        ShakeSpring.OnSpringUpdated += r => BackgroundImage.transform.localRotation = Quaternion.Euler(0, 0, r);

        targetHealthPool.OnModified += OnHealthModified;
    }

    void Update()
    {
        FillSpring.Step(Time.deltaTime);
        FillColorSpring.Step(Time.deltaTime);
        ShakeSpring.Step(Time.deltaTime);

        if (FillSpring.RestingPos <= AlwaysVisibleBelowThreshold) lastHealthEventTime = Time.time;
        var t = Mathf.Clamp01((Time.time - lastHealthEventTime - FadeOutDelay) / FadeOutDuration);
        CanvasGroup.alpha = 1 - t;
        if (t >= 1) gameObject.SetActive(false);
    }
    void OnDestroy() { if (targetHealthPool) targetHealthPool.OnModified -= OnHealthModified; }
    void OnHealthModified(int delta)
    {
        if (delta == 0) return; //ignore health events that didnt actually change the health cuz the player was already at max/min health
        var percentualDelta = delta / (float)targetHealthPool.Size;
        var shakeBaseIntensity = Mathf.Lerp(MinShakeStrength, MaxShakeStrength, percentualDelta);
        var shakeDirection = Random.Range(0, 2) * 2 - 1;
        var shakeVariance = 1f - Random.value * .2f;
        ShakeSpring.Velocity = shakeBaseIntensity * shakeDirection * shakeVariance;
        FillSpring.RestingPos = targetHealthPool.Current / (float)targetHealthPool.Size;
        FillColorSpring.RestingPos = targetHealthPool.Current / (float)targetHealthPool.Size;
        gameObject.SetActive(true);
        lastHealthEventTime = Time.time;
    }
}
