using System;
using System.Collections.Generic;

public class UIToggleButtonGroup : UIBase
{
    public UIToggleButton DefaultActive;
    [GetComponentInChildren, NonSerialized] public List<UIToggleButton> ToggleButtonList;

    protected override void OnEnable()
    {
        base.OnEnable();
        foreach (var toggleButton in ToggleButtonList)
        {
            toggleButton.ToggleButtonGroup = this;
        }

        foreach (var toggleButton in ToggleButtonList)
        {
            toggleButton.SetState(DefaultActive == toggleButton, true);
        }
    }

    public virtual void RefreshToggleGroup(UIToggleButton activeToggleButton)
    {
        foreach (var toggleButton in ToggleButtonList)
        {
            if (toggleButton != activeToggleButton)
            {
                toggleButton.SetState(false);
            }
        }
    }
}