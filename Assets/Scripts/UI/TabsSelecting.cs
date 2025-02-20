using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TabsSelecting : MonoBehaviour
{
    public List<GameObject> tabs = new();
    public List<GameObject> disabledOnWeb = new();

    private void Start()
    {
#if UNITY_WEBGL
        
        disabledOnWeb.ForEach(item =>
        {
            if (item.TryGetComponent(out Button button))
                button.interactable = false;
        });
        
#endif
        
    }

    private void OnEnable() => SetActiveTab(tabs.First());

    public void SetActiveTab(GameObject tab)
    {
        if (!tabs.Contains(tab))
            return;
        
        tabs.ForEach(item => item.SetActive(item == tab));
    }
}