using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TabsGroup : MonoBehaviour
{
    
    [SerializeField] private Tab[] tabs;
    private int indexSelectedTabs = 0;

    [SerializeField] private Color tabIdle;
    [SerializeField] private Color tabHover;
    [SerializeField] private Color tabSelected;

    private void OnEnable()
    {
        indexSelectedTabs = 0;
        onTabSelected(tabs[indexSelectedTabs]);
        ResetTabs();
    }

    public void onTabEnter(Tab tabButton)
    {
        ResetTabs();
        if (Array.IndexOf(tabs, tabButton) != indexSelectedTabs)
            tabButton.background.color = tabHover;
    }

    public void onTabExit(Tab tabButton)
    {
        ResetTabs();
    }

    public void onTabSelected(Tab tabButton)
    {
        indexSelectedTabs = Array.IndexOf(tabs, tabButton);

        Selected(tabButton.type);

        ResetTabs();
        tabButton.background.color = tabSelected;
    }

    private void ResetTabs()
    {
        for(int i = 0; i < tabs.Length; i++)
        {
            if (i == indexSelectedTabs) continue;
            tabs[i].background.color = tabIdle;
        }
    }

    private void Selected(CraftType typeSelected)
    {
        CraftingManager.instance.SetActiveItemCraft(typeSelected);
    }
}
