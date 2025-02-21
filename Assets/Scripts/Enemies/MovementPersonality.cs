using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MovementPersonality : MonoBehaviour
{
    [SerializeField] AnimationCurve movementProfile;

    Cached<VelocityController> cached_VC;
    VelocityController VC => cached_VC[this];

    float spawnTime = 0;
    void OnEnable() => spawnTime = Time.time;

    float areaUnderCurve;
    float walkAnimationDuration;

    void Awake()
    {
        walkAnimationDuration = 1f;
        if (TryGetComponent<CharacterAnimator>(out var animator))
        {
            var maxFrameRate = animator.Animations.Max(a => a.FrameRate);
            var maxFrames = animator.Animations.Max(a => a.Sprites.Length);
            walkAnimationDuration = maxFrames / (float)maxFrameRate;
        }

        areaUnderCurve = CalcAreaUnderCurve(Mathf.RoundToInt(walkAnimationDuration / Time.fixedDeltaTime));
        VC.AddOverwriteMovement(new(new Vector3(1, 0, 1), (t) => 1.414f * movementProfile.Evaluate(((Time.time - spawnTime) / walkAnimationDuration) % 1f) / areaUnderCurve, VelocityBlendMode.Multiplicative), float.PositiveInfinity, 1);
    }

    float CalcAreaUnderCurve(int samples)
    {
        float area = 0;
        for (int i = 0; i < samples; i++)
        {
            float t0 = (i + .5f) / samples;
            area += movementProfile.Evaluate(t0);
        }
        return area / samples;
    }
}
