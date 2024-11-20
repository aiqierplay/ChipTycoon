using System;
using System.Collections.Generic;
using System.Reflection;
using Aya.Extension;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public class UpgradePropertyCollectionItemData
{
    [TableColumnWidth(100, false)]
    public string TargetProperty;

    [TableColumnWidth(100, false)]
    [ValueDropdown(nameof(GetDataPropertyList))]
    public string DataProperty = "Value";

    public IEnumerable<string> GetDataPropertyList()
    {
        if (Type == null) return null;
        return Type.GetType().GetFields().Select(f => f.Name);
    }

    [SerializeReference]
    public ConfigData Type;

    [NonSerialized] public FieldInfo CacheTargetFieldInfo;
    [NonSerialized] public FieldInfo CacheDataFieldInfo;
}
