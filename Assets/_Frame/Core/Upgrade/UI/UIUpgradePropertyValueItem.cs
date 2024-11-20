using System;
using System.Collections;
using Aya.Extension;
using Sirenix.OdinInspector;
using TMPro;

[Serializable]
public class UIUpgradePropertyValueItem
{
    [ValueDropdown(nameof(GetPropertyList))]
    public string Property;

    [VerticalGroup("Value")] public TMP_Text Preview;
    [VerticalGroup("Value")] public TMP_Text Current;
    [VerticalGroup("Value")] public TMP_Text Next;

    [NonSerialized] public Type DataType;

    public IEnumerable GetPropertyList()
    {
        if (DataType == null) return null;
        return DataType.GetFields().Select(f => f.Name);
    }
}