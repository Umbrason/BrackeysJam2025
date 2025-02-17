using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UpgradeSelection : MonoBehaviour
{
    [SerializeField] private UpgradeCard CardTemplate;
    [SerializeField] private RectTransform CardContainer;

    readonly List<UpgradeCard> instances = new();

    public void Show(params IUpgrade[] options)
    {
        if (options.Length == 0) return; //nothing to show
        foreach (var option in options)
            SpawnCard(option);
        gameObject.SetActive(true);
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
            if (instanceID >= enabledInstances)
            {
                instances[instanceID].enabled = true;
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
            if (!anyNotDone) yield break;
            yield return null;
        }
    }

    public void Close()
    {
        gameObject.SetActive(false);
        DestroyCards();
        Time.timeScale = 1;
    }

    private void DestroyCards()
    {
        foreach (var instance in instances)
            Destroy(instance.gameObject);
        instances.Clear();
    }

    private void SpawnCard(IUpgrade upgrade)
    {
        var instance = Instantiate(CardTemplate, CardContainer ?? transform);
        instance.DisplayedUpgrade = upgrade;
        instance.OnClicked += OnOptionClicked;
        instances.Add(instance);
        instance.enabled = false;
    }

    private bool acceptClicks = true; //used to disable clicking during fade in animations
    private void OnOptionClicked(UpgradeCard card)
    {
        //TODO: add correct target object here
        if (acceptClicks)
            card.DisplayedUpgrade.OnApply(null);
        Close();
    }
}