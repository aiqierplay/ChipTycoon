using System;
using UnityEngine;
using Sirenix.OdinInspector;
#if UNITY_EDITOR
using UnityEditor;
#endif

[Serializable]
public class SubSettingBase<TSettingCollection, TSubSetting> : ScriptableObject
    where TSettingCollection : SubSettingRootBase<TSettingCollection, TSubSetting>
    where TSubSetting : SubSettingBase<TSettingCollection, TSubSetting>
{
    [ReadOnly]
    [BoxGroup("Sub Setting")]
    public TSettingCollection RootSetting;

    [GUIColor(0.5f, 1f, 1f)]
    [BoxGroup("Sub Setting")]
    public string Key;

    #region Sub Setting Operation

#if UNITY_EDITOR
    public void OnValidate()
    {
        if (name == Key) return;
        name = Key;
        AssetDatabase.SaveAssets();
        EditorUtility.SetDirty(this);
    }

    [Button(SdfIconType.Trash)]
    [BoxGroup("Sub Setting")]
    [GUIColor(1f, 0.75f, 0.75f)]
    public void DeleteSubSetting()
    {
        RootSetting.Delete(this as TSubSetting);
    }
#endif

    #endregion
}