using System;
using System.Collections.Generic;
using System.Reflection;
using Aya.Extension;
using Sirenix.OdinInspector;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[LabelWidth(120)]
public class UIDataBinder : UIBase
{
    [BoxGroup("Data Binder")]
    [HorizontalGroup("Data Binder/Type")]
    [LabelWidth(80)]
    [LabelText(nameof(SourceType))]
    [GUIColor(nameof(GetTypeColor))]
    [ValueDropdown(nameof(GetDataTypeList))]
    public string Type;

#if UNITY_EDITOR
    [BoxGroup("Data Binder")]
    [HorizontalGroup("Data Binder/Type", 60)]
    [Button(SdfIconType.PencilSquare, " Edit")]
    [ShowIf(nameof(Type), null)]
    public void EditScript()
    {
        var fileName = Type;
        var guidArray = AssetDatabase.FindAssets(fileName);
        if (guidArray.Length == 0) return;
        var scriptGuid = guidArray.First(path =>
        {
            var assetPath = AssetDatabase.GUIDToAssetPath(path);
            var extension = System.IO.Path.GetExtension(assetPath).ToLower();
            if (extension == ".cs") return true;
            return false;
        });

        if (string.IsNullOrEmpty(scriptGuid)) return;
        var scriptPath = AssetDatabase.GUIDToAssetPath(scriptGuid);
        var scriptAsset = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(scriptPath);
        if (scriptAsset == null) return;
        AssetDatabase.OpenAsset(scriptAsset);
    }
#endif

    public Color GetTypeColor() => string.IsNullOrEmpty(Type) ? new Color(1f, 0.5f, 0.5f) : new Color(0.5f, 1f, 1f);

    public ValueDropdownList<string> GetDataTypeList()
    {
        var list = new ValueDropdownList<string>();
        var types = Assembly.GetExecutingAssembly().GetTypes();
        list.Add("NULL", "");
        foreach (var type in types)
        {
            var item = new ValueDropdownItem<string>(type.Name, type.Name);
            list.Add(item);
        }

        return list;
    }

    [BoxGroup("Data Binder")]
    [LabelWidth(80)]
    public List<UIDataBinderData> DataList;

    [NonSerialized] public Type SourceType;
    [NonSerialized] public object SourceData;

    public virtual void RefreshCache()
    {
        if ((SourceType == null || SourceType.Name != Type) && !string.IsNullOrEmpty(Type))
        {
            SourceType = Assembly.GetExecutingAssembly().GetType(Type);
        }

        if (DataList == null) return;
        foreach (var data in DataList)
        {
            data.Owner = gameObject;
            data.SourceData = SourceData;
            data.SourceType = SourceType;
            data.RefreshCache();
        }
    }

    public override void Refresh(bool immediately = false)
    {
        base.Refresh(immediately);
        RefreshData();
    }

    public virtual void SetSourceData(object data, bool refresh = true)
    {
        SourceData = data;
        SourceType = data.GetType();
        if (refresh) Refresh();
    }

    public virtual void RefreshData()
    {
        if (SourceData == null) return;
        RefreshCache();
        if (DataList == null) return;
        foreach (var data in DataList)
        {
            var value = data.GetSourceValue();
            data.SetTargetValue(value);
        }
    }

#if UNITY_EDITOR

    public void OnValidate()
    {
        RefreshCache();
    }

    [BoxGroup("Data Binder")]
    [Button(SdfIconType.ArrowRepeat, " Auto Cache")]
    public void AutoCache()
    {
        RefreshCache();
        if (SourceType == null) return;
        var member = SourceType.GetMembers();
        var components = GetComponentsInChildren<Component>();
        foreach (var memberInfo in member)
        {
            if (memberInfo.MemberType != MemberTypes.Field && memberInfo.MemberType != MemberTypes.Property) continue;
            var dataProperty = memberInfo.Name;
            var dataType = memberInfo.GetMemberType();
            if (dataType == null) continue;
            for (var i = 0; i < components.Length; i++)
            {
                var target = components[i];
                var gameObj = target.gameObject;
                var gameObjName = gameObj.name;
                var targetType = target.GetType();
                if (gameObjName == dataProperty ||
                    gameObjName.Contains(dataProperty) ||
                    gameObjName.ToLower().Contains(dataProperty.ToLower()) ||
                    dataProperty.ToLower().Contains(gameObjName.ToLower()))
                {
                    if (UIDataBinderMap.AutoCacheTypeDic.TryGetValue(dataType, targetType, out var targetProperty))
                    {
                        AutoCache(dataProperty, target, targetProperty);
                    }
                }
            }
        }

        RefreshCache();
    }

    public void AutoCache(string dataProperty, Component target, string targetProperty)
    {
        DataList.Add(new UIDataBinderData()
        {
            SourceProperty = dataProperty,
            Target = target,
            TargetProperty = targetProperty
        });
    }

#endif
}
