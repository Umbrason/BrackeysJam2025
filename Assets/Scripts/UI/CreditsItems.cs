using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreditsItems : MonoBehaviour
{
    [Header("References")]
    public Image icon;
    public TMP_Text nameTMP, descriptionTMP;
    public GameObject linkPrefab;
    public Transform linksParent;
    
    [Header("Slave")]
    public AffiliatePerson person;
    
    [EasyButtons.Button]
    public void Start()
    {
        while (linksParent.childCount > 0) // remove all links
            Destroy(linksParent.GetChild(0));
        
        icon.sprite = person.icon;
        nameTMP.SetText(person.name);
        descriptionTMP.SetText(person.description);
        person.links.ForEach(link =>
        {
            var obj = Instantiate(linkPrefab, linksParent);
            link.Set(obj);
        });
        
        RefreshLayoutGroupsImmediateAndRecursive(transform.parent.gameObject);
    }
    
    // https://discussions.unity.com/t/layoutgroup-does-not-refresh-in-its-current-frame/656699/7
    public void RefreshLayoutGroupsImmediateAndRecursive(GameObject root)
    {
        var componentsInChildren = root.GetComponentsInChildren<LayoutGroup>(true);
        foreach (var layoutGroup in componentsInChildren)
            LayoutRebuilder.ForceRebuildLayoutImmediate(layoutGroup.GetComponent<RectTransform>());

        if (root.TryGetComponent(out LayoutGroup parent))
            LayoutRebuilder.ForceRebuildLayoutImmediate(parent.GetComponent<RectTransform>());
    }
}

[Serializable]
public class AffiliatePerson
{
    public Sprite icon;
    [TextArea] public string name;
    [TextArea] public string description;
    public List<AffiliateLink> links;
}

[Serializable]
public class AffiliateLink
{
    public Sprite icon;
    [TextArea] public string link;

    public void Set(GameObject baseItem)
    {
        Image img = baseItem.GetComponent<Image>();
        Button btn = baseItem.GetComponent<Button>();
        
        img.sprite = icon;
        btn.onClick.AddListener(() => Application.OpenURL(link));
    }
}