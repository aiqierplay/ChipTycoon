using System;
using System.Collections.Generic;
using Aya.UI;
using Sirenix.OdinInspector;

public class UITab : UIBase
{
    public int DefaultPageIndex = 0;

    [TableList] public List<UITabItem> Tabs = new List<UITabItem>();
    [NonSerialized] public int PageIndex;

    protected override void Awake()
    {
        base.Awake();
        for (var i = 0; i < Tabs.Count; i++)
        {
            var tab = Tabs[i];
            var listener = UIEventListener.Get(tab.TabButton);
            tab.Index = i;
            listener.onClick += (go, data) =>
            {
                SwitchTab(tab.Index);
                Dispatch(UIEvent.SwitchTab, PageIndex);
            };
        }

        SwitchTab(DefaultPageIndex, true);
    }

    public virtual void SwitchTab(int index, bool immediately = false)
    {
        if (index == PageIndex) return;
        if (index >= Tabs.Count) return;
        if (PageIndex >= 0 && PageIndex < Tabs.Count) Tabs[PageIndex].OnTabDeSelectAction();
        for (var i = 0; i < Tabs.Count; i++)
        {
            Tabs[i].TabPanel.SetActive(i == index);
        }

        Tabs[index].OnTabSelectAction();
        PageIndex = index;
    }
}