using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif

[Serializable]
public abstract class SubSettingRootBase<TSettingCollection, TSubSetting> : SettingBase<TSettingCollection>
    where TSettingCollection : SubSettingRootBase<TSettingCollection, TSubSetting>
    where TSubSetting : SubSettingBase<TSettingCollection, TSubSetting>
{
    [ReadOnly]
    [BoxGroup("Sub Setting")]
    public List<TSubSetting> DataList;

    public Dictionary<string, TSubSetting> DataDic
    {
        get
        {
            if (DataDicCache != null) return DataDicCache;
            DataDicCache = DataList.ToDictionary(subSetting => subSetting.Key);
            return DataDicCache;
        }
    }

    protected Dictionary<string, TSubSetting> DataDicCache;

    public ValueDropdownList<string> GetValueDropdownKeyList()
    {
        var result = new ValueDropdownList<string>();
        foreach (var subSetting in DataList)
        {
            var item = new ValueDropdownItem<string>(subSetting.Key, subSetting.Key);
            result.Add(item);
        }

        return result;
    }


    #region Sub Setting Operation

#if UNITY_EDITOR
    [Button(SdfIconType.FileEarmarkPlus)]
    [BoxGroup("Sub Setting")]
    [GUIColor(0.75f, 1f, 0.75f)]
    public void CreateSubSetting()
    {
        var subSetting = CreateInstance<TSubSetting>();
        var key = typeof(TSubSetting).Name + "_" + (DataList.Count + 1);
        subSetting.name = key;
        subSetting.Key = key;
        subSetting.RootSetting = this as TSettingCollection;

        AssetDatabase.AddObjectToAsset(subSetting, this);
        DataList.Add(subSetting);

        AssetDatabase.SaveAssets();
        EditorUtility.SetDirty(this);
    }

    [Button(SdfIconType.Trash)]
    [BoxGroup("Sub Setting")]
    [GUIColor(1f, 0.75f, 0.75f)]
    public void DeleteAllSubSetting()
    {
        foreach (var subSetting in DataList)
        {
            if (subSetting == null) continue;
            DestroyImmediate(subSetting, true);
        }

        DataList.Clear();
        AssetDatabase.SaveAssets();
        EditorUtility.SetDirty(this);
    }

    public void Delete(TSubSetting subSetting)
    {
        if (!DataList.Contains(subSetting)) return;
        DataList.Remove(subSetting);
        DestroyImmediate(subSetting, true);

        AssetDatabase.SaveAssets();
        EditorUtility.SetDirty(this);

        Selection.activeObject = this;
    }
#endif

    #endregion
}