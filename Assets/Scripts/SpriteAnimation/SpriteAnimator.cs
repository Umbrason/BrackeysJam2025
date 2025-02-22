using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteAnimator : MonoBehaviour, ISpriteAnimator
{
    [SerializeField] private Sprite[] m_Sprites;
    public Sprite[] Sprites
    {
        get => m_Sprites;
        set
        {
            m_Sprites = value;
            time = 0;
        }
    }
    [field: SerializeField] public int Framerate { get; set; }
    [field: SerializeField] public float SpeedMultiplier { get; set; } = 1;
    private float time;

    Cached<SpriteRenderer> cached_SpriteRenderer;
    SpriteRenderer SpriteRenderer => cached_SpriteRenderer[this];
    public bool FlipX { get => SpriteRenderer.transform.localScale.x < 0; set => SpriteRenderer.transform.localScale = new((value ? -1 : 1) * Mathf.Abs(SpriteRenderer.transform.localScale.x), SpriteRenderer.transform.localScale.y, SpriteRenderer.transform.localScale.z); }

    void Update()
    {
        if (SpriteRenderer == null || Sprites?.Length == 0) return;
        time += Time.deltaTime * SpeedMultiplier;
        var frameID = Mathf.FloorToInt(Framerate * time);
        SpriteRenderer.sprite = Sprites[frameID % Sprites.Length];
    }
}
