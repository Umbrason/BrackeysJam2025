using UnityEngine;

[CreateAssetMenu(menuName = "AnimationSet")]
public class AnimationSet : ScriptableObject
{
    [field: SerializeField] public int FrameRate { get; private set; }
    [field: SerializeField] public Sprite[] Sprites { get; private set; }
    [field: SerializeField] public bool MirrorX { get; private set; }
}