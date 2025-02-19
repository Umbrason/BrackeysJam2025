using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardboardSpriteAnimator : MonoBehaviour, ISpriteAnimator
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
    [SerializeField] private float time;


    Cached<CardboardSpriteRenderer> cached_SpriteRenderer;
    CardboardSpriteRenderer SpriteRenderer => cached_SpriteRenderer[this];
    public bool FlipX { get => SpriteRenderer.transform.localScale.x < 0; set => SpriteRenderer.transform.localScale = new(value ? -1 : 1, 1, 1); }

    void Update()
    {
        if (SpriteRenderer == null || Sprites?.Length == 0) return;
        time += Time.deltaTime * SpeedMultiplier;
        var frameID = Mathf.FloorToInt(Framerate * time);
        SpriteRenderer.Sprite = Sprites[frameID % Sprites.Length];
    }
}
