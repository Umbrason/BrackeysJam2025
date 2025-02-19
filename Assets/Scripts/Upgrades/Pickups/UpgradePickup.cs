using System.Collections;
using System.Linq;
using UnityEngine;

public class UpgradePickup : MonoBehaviour
{
    void OnTriggerEnter(Collider collider) => TriggerPickUp(collider?.attachedRigidbody?.gameObject);
    void TriggerPickUp(GameObject target)
    {
        if (pickingUp) return;
        pickingUp = true;
        StartCoroutine(PickupRoutine(target));
    }
    bool pickingUp = false;
    const int UpgradeOptionCount = 3;
    [SerializeField] private Sprite[] OpenAnimation;
    [SerializeField] private int openAnimationFramerate;
    Cached<ISpriteAnimator> cached_Animator = new(Cached<ISpriteAnimator>.GetOption.Children);
    ISpriteAnimator Animator => cached_Animator[this];
    IEnumerator PickupRoutine(GameObject target)
    {
        var options = Enumerable.Range(0, UpgradeOptionCount).Select(_ => IUpgrade.UpgradePool.Pull()).ToArray();
        Animator.Framerate = openAnimationFramerate;
        Animator.Sprites = OpenAnimation;
        yield return new WaitForSeconds(OpenAnimation.Length / (float)openAnimationFramerate);
        UpgradeSelection.Show(target, options);
    }
}