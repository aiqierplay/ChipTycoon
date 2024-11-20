#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EditorSettingBase<T> : ScriptableObject
    where T : EditorSettingBase<T>
{
    #region Instance

    public static T Ins
    {
        get
        {
            if (Instance == null) Instance = LoadAsset();
            return Instance;
        }
    }

    protected static T Instance;

    public static T LoadAsset()
    {
        var guidList = AssetDatabase.FindAssets("t:" + typeof(T).FullName);
        if (guidList != null && guidList.Length > 0)
        {
            var setting = AssetDatabase.LoadAssetAtPath<T>(AssetDatabase.GUIDToAssetPath(guidList[0]));
            setting.Init();
            return setting;
        }

        return null;
    }

    public static List<T> LoadAllAsset()
    {
        var result = new List<T>();
        var guidList = AssetDatabase.FindAssets("t:" + typeof(T).FullName);
        if (guidList != null && guidList.Length > 0)
        {
            for (var i = 0; i < guidList.Length; i++)
            {
                var setting = AssetDatabase.LoadAssetAtPath<T>(AssetDatabase.GUIDToAssetPath(guidList[i]));
                setting.Init();
                result.Add(setting);
            }
        }

        return result;
    }

    #endregion

    public virtual void Init()
    {

    }
}
#endif