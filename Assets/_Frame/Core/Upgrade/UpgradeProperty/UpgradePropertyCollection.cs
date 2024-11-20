using System;
using System.Collections.Generic;
using Aya.Events;
using Aya.Extension;
using Sirenix.OdinInspector;

public class UpgradePropertyCollection : EntityBase
{
    [GUIColor(0f, 1f, 1f)] public string AssetPath;
    public bool SaveByLevel;
    [TableList] public List<UpgradePropertyCollectionItemData> PropertyList;

    [NonSerialized] public Dictionary<string, UpgradePropertyCollectionItemData> PropertyCacheDic;
    [NonSerialized] public EntityBase Owner;

    [HideInEditorMode, ReadOnly, ShowInInspector, NonSerialized] public string SaveKey;

    public void Init(EntityBase owner)
    {
        Owner = owner;
        PropertyCacheDic = PropertyList.ToDictionary(p => p.TargetProperty);

        SaveKey = AssetPath;
        if (SaveByLevel) SaveKey = $"{CurrentLevel.SaveKey}/{AssetPath}";

        var ownerType = Owner.GetType();
        foreach (var upgradeProperty in PropertyList)
        {
            var targetFieldInfo = ownerType.GetField(upgradeProperty.TargetProperty);
            upgradeProperty.CacheTargetFieldInfo = targetFieldInfo;
            var dataFieldInfo = upgradeProperty.Type.GetType().GetField(upgradeProperty.DataProperty);
            upgradeProperty.CacheDataFieldInfo = dataFieldInfo;
        }

        RefreshProperty();
    }

    [Listen(GameEvent.Upgrade)]
    public void RefreshProperty()
    {
        if (Owner == null) return;
        foreach (var upgradeProperty in PropertyList)
        {
            var dataType = upgradeProperty.Type.GetType();
            var info = Upgrade.GetInfo(dataType, SaveKey);
            var targetFieldInfo = upgradeProperty.CacheTargetFieldInfo;
            var dataFieldInfo = upgradeProperty.CacheDataFieldInfo;
            var dataValue = dataFieldInfo.GetValue(info.Current);
            if (targetFieldInfo.FieldType != dataFieldInfo.FieldType)
            {
                var value = dataValue.CastType(targetFieldInfo.FieldType);
                targetFieldInfo.SetValue(Owner, value);
            }
            else
            {
                targetFieldInfo.SetValue(Owner, dataValue);
            }
        }
    }

    public UpgradeInfo<TData> GetInfo<TData>(string property) where TData : ConfigData
    {
        var dataType = typeof(TData);
        return GetInfo(dataType, property) as UpgradeInfo<TData>;
    }

    public UpgradeInfo GetInfo(Type dataType, string property)
    {
        if (!PropertyCacheDic.TryGetValue(property, out var upgradeProperty)) return default;
        var info = Upgrade.GetInfo(dataType, SaveKey);
        return info;
    }
}