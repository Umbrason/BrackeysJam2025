using System.Collections;
using System.Linq;
using UnityEngine;

public class UpgradePickup : MonoBehaviour
{
    public static UpgradePickup Instance;
    void Awake() => Instance = this;
    void OnDestroy() => Instance = null;
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
    [SerializeField] AudioClipGroup upgradeVoiceLines;
    IEnumerator PickupRoutine(GameObject target)
    {
        var options = Enumerable.Range(0, UpgradeOptionCount + (AlienEye.IsActive ? 1 : 0)).Select(_ => IUpgrade.UpgradePool.Pull()).ToArray();
        Animator.Framerate = openAnimationFramerate;
        Animator.Sprites = OpenAnimation;
        VoicelinePlayer.Play(upgradeVoiceLines);
        yield return new WaitForSeconds(OpenAnimation.Length / (float)openAnimationFramerate);
        UpgradeSelection.Show(target, options);
        Destroy(gameObject);
    }
}