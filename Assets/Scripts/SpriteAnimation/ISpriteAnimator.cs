using UnityEngine;

public interface ISpriteAnimator
{
    Sprite[] Sprites { get; set; }
    int Framerate { get; set; }
}