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

    const float enableCardsAnimationDuration = 2f;
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
    }

    const float pauseGameAnimationDuration = .5f;
    private IEnumerator PauseGameRoutine()
    {
        var t = 0f;
        while (t < 1)
        {
            Time.timeScale = 1 - t;
            t += Time.unscaledDeltaTime / pauseGameAnimationDuration;
            yield return null;
        }
        Time.timeScale = 0;
    }

    private IEnumerator ShowRoutine()
    {
        IEnumerator[] enumerators = {
            PauseGameRoutine(),
            EnableCardsRoutine()
        };
        while (true)
        {
            var anyNotDone = false;
            foreach (var enumerator in enumerators)
                anyNotDone |= enumerator.MoveNext();
            if (!anyNotDone) break;
            yield return null;
        }
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
        instances.Clear();
        gameObject.SetActive(false);
        Time.timeScale = 1;
        Cursor.visible = cursorWasVisible;
    }
    const float hideCardAnimationDuration = 1f;
    private IEnumerator HideCardRoutine(UpgradeCard card)
    {
        card.Hide();
        var t = 0f;
        while (t < 1)
        {
            Time.timeScale = 1 - t;
            t += Time.unscaledDeltaTime / hideCardAnimationDuration;
            yield return null;
        }
        Time.timeScale = 0;
        Destroy(card);
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
        if (acceptClicks) card.DisplayedUpgrade.OnApply(upgradeTarget);
        card.Shake();
        Close(card);
    }
}