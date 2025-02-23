using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UpgradeSelection : MonoBehaviour
{
    [SerializeField] private UpgradeCard CardTemplate;
    [SerializeField] private RectTransform CardContainer;

    readonly List<UpgradeCard> instances = new();

    private static UpgradeSelection Instance;
    void Awake()
    {
        Instance = Instance == null ? this : Instance;
        gameObject.SetActive(false);
    }
    void OnDestroy() => Instance = this == Instance ? null : Instance;

    public static void Show(GameObject target, params IUpgrade[] options) => Instance.ShowInstance(target, options);
    bool cursorWasVisible = false;
    GameObject upgradeTarget;
    private void ShowInstance(GameObject target, params IUpgrade[] options)
    {
        if (options.Length == 0) return; //nothing to show
        foreach (var option in options)
            SpawnCard(option);
        this.upgradeTarget = target;
        gameObject.SetActive(true);
        cursorWasVisible = Cursor.visible;
        Cursor.visible = true;
        StartCoroutine(ShowRoutine());
    }

    const float enableCardsAnimationDuration = 1.5f;
    private IEnumerator EnableCardsRoutine()
    {
        var t = 0f;
        var enabledInstances = 0;
        while (t <= 1)
        {
            var instanceID = Mathf.FloorToInt(t * (instances.Count - 1));
            while (instanceID >= enabledInstances)
            {
                instances[enabledInstances].enabled = true;
                enabledInstances++;
            }
            t += Time.unscaledDeltaTime / enableCardsAnimationDuration;
            yield return null;
        }
        instances[^1].enabled = true;
        foreach (var instance in instances)
            instance.Disabled = false;
    }

    Guid pauseHandle;
    private IEnumerator ShowRoutine()
    {
        pauseHandle = Pause.Request();
        yield return EnableCardsRoutine();
        acceptClicks = true;
    }

    private IEnumerator CloseRoutine(UpgradeCard chosenCard)
    {
        foreach (var card in instances)
        {
            if (card == chosenCard) continue;
            yield return HideCardRoutine(card);
        }
        yield return HideCardRoutine(chosenCard);
        foreach (var instance in instances)
            Destroy(instance.gameObject);
        instances.Clear();
        gameObject.SetActive(false);
        Pause.Return(pauseHandle);
        Cursor.visible = cursorWasVisible;
        var targetHealthpool = upgradeTarget.GetComponent<HealthPool>();
        targetHealthpool.RegisterHealthEvent(HealthEvent.Heal((uint)targetHealthpool.Size, source: null));
        upgradeTarget = null;
    }
    const float hideCardAnimationDuration = .5f;
    private IEnumerator HideCardRoutine(UpgradeCard card)
    {
        card.Hide();
        var t = 0f;
        while (t < 1)
        {
            t += Time.unscaledDeltaTime / hideCardAnimationDuration;
            yield return null;
        }
    }

    public void Close(UpgradeCard card)
    {
        StartCoroutine(CloseRoutine(card));
    }

    private void SpawnCard(IUpgrade upgrade)
    {
        var instance = Instantiate(CardTemplate, CardContainer ?? transform);
        instance.DisplayedUpgrade = upgrade;
        instance.OnClicked += OnOptionClicked;
        instances.Add(instance);
        instance.enabled = false;
    }

    private bool acceptClicks = false; //used to disable clicking during fade in animations
    private void OnOptionClicked(UpgradeCard card)
    {
        if (!acceptClicks) return;
        foreach (var instance in instances)
            instance.Disabled = true;
        card.DisplayedUpgrade.OnApply(upgradeTarget);
        UpgradeLog.Log(card.DisplayedUpgrade);
        TransientScoring.AddUpgradesCollected(1);
        if (card.DisplayedUpgrade.UpgradeVoiceLine != null)
            VoicelinePlayer.Play(card.DisplayedUpgrade.UpgradeVoiceLine, true);
        if (!card.DisplayedUpgrade.Stackable) IUpgrade.UpgradePool.Remove(card.DisplayedUpgrade);
        acceptClicks = false;
        card.Shake();
        Close(card);
    }
}