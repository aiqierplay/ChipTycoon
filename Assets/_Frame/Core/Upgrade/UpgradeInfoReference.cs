using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Reflection;
using Aya.Extension;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[Serializable]
[BoxGroup]
public class UpgradeInfoReference
{
    [ValueDropdown(nameof(TypeGetter))]
    [GUIColor(nameof(TypeColorGetter))]
    public string Type;
    public bool SaveByLevel;

    public List<ValueDropdownItem<string>> TypeGetter()
    {
#if !UNITY_EDITOR
        return null;
#else
        var typeList = typeof(CostConfigData)
            .GetSubTypes()
            .FindAll(t => !t.IsAbstract)
            .Select(type => new ValueDropdownItem<string>(ObjectNames.NicifyVariableName(type.Name), type.Name))
            .ToList();
        typeList.Insert(0, new ValueDropdownItem<string>("<None>", ""));
        return typeList;
#endif
    }

    public Color TypeColorGetter()
    {
        return string.IsNullOrEmpty(Type) ? new Color(1f, 0.5f, 0.5f) : new Color(0.5f, 1f, 1f);
    }

    [EnumToggleButtons] public UpgradeSaveMode SaveMode = UpgradeSaveMode.Default;
    [ShowIf(nameof(SaveMode), UpgradeSaveMode.Custom)]
    [GUIColor(0f, 1f, 1f)]
    public string CustomSaveKey;

    [EnumToggleButtons] public UpgradeDataMode DataMode = UpgradeDataMode.Default;
    [ShowIf(nameof(DataMode), UpgradeDataMode.Asset)]
    [GUIColor(0f, 1f, 1f)]
    public TextAsset DataAsset;
    [ShowIf(nameof(DataMode), UpgradeDataMode.Path)]
    [GUIColor(0f, 1f, 1f)]
    public string AssetPath;

    [NonSerialized] public bool IsInit = false;

    [NonSerialized] public Type DataType;
    [HideInEditorMode, ReadOnly, ShowInInspector, NonSerialized] public string SaveKey;

    [NonSerialized] public CostConfigData Data;
    [NonSerialized] public UpgradeInfo Info;

    public void Init()
    {
        var upgradeManager = UpgradeManager.Ins;
        var levelManager = LevelManager.Ins;
        if (upgradeManager == null || levelManager == null) return;
        var currentLevel = LevelManager.Ins.CurrentLevel;
        if (currentLevel == null) return;
        DataType = Assembly.GetExecutingAssembly().GetType(Type);
        SaveKey = SaveByLevel ? currentLevel.SaveKey : "";

        switch (SaveMode)
        {
            case UpgradeSaveMode.Default:
                break;
            case UpgradeSaveMode.Custom:
                SaveKey = $"{SaveKey}/{CustomSaveKey}";
                break;
        }

        switch (DataMode)
        {
            case UpgradeDataMode.Default:
                Info = upgradeManager.GetInfo(DataType, SaveKey);
                if (Info == null) return;
                Data = Info.Current as CostConfigData;
                break;

            case UpgradeDataMode.Asset:
                Info = upgradeManager.GetInfo(DataAsset, DataType, SaveKey);
                if (Info == null) return;
                Data = Info.Current as CostConfigData;
                break;

            case UpgradeDataMode.Path:
                if (string.IsNullOrEmpty(SaveKey)) SaveKey = AssetPath;
                else SaveKey = $"{SaveKey}/{AssetPath}";
                Info = upgradeManager.GetInfo(DataType, SaveKey);
                Data = Info.Current as CostConfigData;
                break;
        }

        IsInit = true;
    }
}
