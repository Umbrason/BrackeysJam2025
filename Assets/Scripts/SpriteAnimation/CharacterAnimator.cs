using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    Cached<ISpriteAnimator> cached_SpriteAnimator;
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
    DirectionalAnimation Idle;

    DirectionalAnimation m_CurrentAnimation;
    DirectionalAnimation CurrentAnimation
    {
        set
        {
            value ??= Idle;
            if (value == m_CurrentAnimation) return;
            if (value == null) return;
            m_CurrentAnimation = value;
            SpriteAnimator.Sprites = value.animation.Sprites;
            SpriteAnimator.Framerate = value.animation.FrameRate;
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
        var vy = RB.velocity.y;
        var movementAngle = Mathf.Atan2(vy, vx);
        for (int i = 0; i < animations.Length; i++)
        {
            var alpha = Mathf.DeltaAngle(animations[i].angle, movementAngle);
            if (bestScore > alpha)
            {
                bestScore = alpha;
                bestAnimation = animations[i];
            }
        }
        CurrentAnimation = bestAnimation;
    }
}