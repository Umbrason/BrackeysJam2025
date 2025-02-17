using System.Collections.Generic;
using UnityEngine;

public class UpgradeSelection : MonoBehaviour
{
    [SerializeField] private UpgradeCard CardTemplate;
    [SerializeField] private RectTransform CardContainer;

    readonly List<UpgradeCard> instances = new();

    public void Show(params IUpgrade[] options)
    {
        foreach (var option in options)
            SpawnCard(option);
        gameObject.SetActive(true);
    }

    public void Close()
    {
        gameObject.SetActive(false);
        DestroyCards();
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
    }

    private bool acceptClicks = true; //used to disable clicking during fade in animations
    private void OnOptionClicked(UpgradeCard card)
    {
        //TODO: add correct target object here
        if(acceptClicks)
        card.DisplayedUpgrade.OnApply(null);
        Close();
    }
}