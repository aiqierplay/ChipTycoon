using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;

[BoxGroup, HideLabel]
[Serializable]
public class ABTestValueReference<T>
{
    [ToggleGroup(nameof(EnableAbTest), "AB Test")]
    public bool EnableAbTest;

    [ToggleGroup(nameof(EnableAbTest), "AB Test")]
    [GUIColor(0.5f, 1f, 1f)]
    public string GroupKey;

    [ToggleGroup(nameof(EnableAbTest), "AB Test")]
    [GUIColor(0.7f, 0.7f, 0.7f)]
    public T DefaultValue;

    [ToggleGroup(nameof(EnableAbTest), "AB Test")]
    public List<ABTestValueReferenceData<T>> List = new List<ABTestValueReferenceData<T>>();

    public T GetValue()
    {
        if (EnableAbTest)
        {
            var group = ABTestSetting.Ins.GetGroup(GroupKey);
            foreach (var data in List)
            {
                data.IsCurrentConfig = data.ConfigKey == group.CurrentKey;
                if (data.IsCurrentConfig) return data.RefValue;
            }

            foreach (var data in List)
            {
                if (data.IsCurrentConfig) return data.RefValue;
            }

            return GetDefaultValue();
        }
        else
        {
            return GetDefaultValue();
        }
    }

    public T GetDefaultValue()
    {
        return DefaultValue;
    }
}