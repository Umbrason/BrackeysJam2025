using System.Collections;
using System.Linq;
using UnityEngine;

public class UpgradePickup : MonoBehaviour
{
    void OnTriggerEnter() => TriggerPickUp();
    void TriggerPickUp()
    {
        if (pickingUp) return;
        pickingUp = true;
        StartCoroutine(PickupRoutine());
    }
    bool pickingUp = false;
    const int UpgradeOptionCount = 3;
    [SerializeField] private Sprite[] OpenAnimation;
    [SerializeField] private int openAnimationFramerate;
    Cached<ISpriteAnimator> cached_Animator = new(Cached<ISpriteAnimator>.GetOption.Children);
    ISpriteAnimator Animator => cached_Animator[this];
    IEnumerator PickupRoutine()
    {
        var options = Enumerable.Range(0, UpgradeOptionCount).Select(_ => IUpgrade.UpgradePool.Pull()).ToArray();
        Animator.Framerate = openAnimationFramerate;
        Animator.Sprites = OpenAnimation;
        yield return new WaitForSeconds(OpenAnimation.Length / (float)openAnimationFramerate);
        UpgradeSelection.Show(options);
    }
}