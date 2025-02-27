using System.Linq;
using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    Cached<ISpriteAnimator> cached_SpriteAnimator = new(Cached<ISpriteAnimator>.GetOption.Children);
    ISpriteAnimator SpriteAnimator => cached_SpriteAnimator[this];
    Cached<Rigidbody> cached_RB;
    Rigidbody RB => cached_RB[this];


    [System.Serializable]
    public class DirectionalAnimation
    {
        public float angle;
        public AnimationSet animation;
    }
    [SerializeField] DirectionalAnimation[] animations;
    [SerializeField] AnimationSet Idle;
    public AnimationSet[] Animations => animations.Select(d => d.animation).Prepend(Idle).ToArray();

    AnimationSet m_CurrentAnimation;
    AnimationSet CurrentAnimation
    {
        set
        {
            value ??= Idle;
            if (value == m_CurrentAnimation) return;
            if (value == null) return;
            m_CurrentAnimation = value;
            SpriteAnimator.Sprites = value.Sprites;
            SpriteAnimator.Framerate = value.FrameRate;
            SpriteAnimator.FlipX = value.MirrorX;
        }
    }

    void FixedUpdate()
    {
        if (RB.velocity.sqrMagnitude <= .05)
        {
            CurrentAnimation = Idle;
            return;
        }
        var bestScore = float.PositiveInfinity;
        var bestAnimation = Idle;
        var vx = RB.velocity.x;
        var vy = RB.velocity.z;
        var movementAngle = Mathf.Atan2(-vy, vx) * 180 / Mathf.PI + 90;
        for (int i = 0; i < animations.Length; i++)
        {
            var alpha = Mathf.Abs(Mathf.DeltaAngle(animations[i].angle, movementAngle));
            if (bestScore > alpha)
            {
                bestScore = alpha;
                bestAnimation = animations[i].animation;
            }
        }
        CurrentAnimation = bestAnimation;
    }
}