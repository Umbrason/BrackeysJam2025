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
    [SerializeField] private float time;


    Cached<CardboardSpriteRenderer> cached_SpriteRenderer;
    CardboardSpriteRenderer SpriteRenderer => cached_SpriteRenderer[this];

    void Update()
    {
        if (SpriteRenderer == null || Sprites?.Length == 0)
            time += Time.deltaTime;
        var frameID = Mathf.FloorToInt(Framerate * time);
        SpriteRenderer.Sprite = Sprites[frameID % Sprites.Length];
    }
}
