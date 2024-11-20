using Sirenix.OdinInspector;
using System;
using System.Reflection;
using UnityEngine;

[Serializable]
public class UIDataBinderData
{
    [ValueDropdown(nameof(GetSourcePropertyList))] 
    [GUIColor(nameof(GetDataPropertyColor))]
    [LabelText("Source")]
    public string SourceProperty;

    public ValueDropdownList<string> GetSourcePropertyList()
    {
        var list = new ValueDropdownList<string> { { "NULL", "" } };
        RefreshCache();
        if (SourceType == null) return list;
        var properties = SourceType.GetProperties();
        foreach (var propertyInfo in properties)
        {
            var item = new ValueDropdownItem<string>("Property / " + propertyInfo.PropertyType.Name + " / " + propertyInfo.Name, propertyInfo.Name);
            list.Add(item);
        }

        var fields = SourceType.GetFields();
        foreach (var fieldInfo in fields)
        {
            var item = new ValueDropdownItem<string>("Filed / " + fieldInfo.FieldType.Name + " / " + fieldInfo.Name, fieldInfo.Name);
            list.Add(item);
        }

        return list;
    }

    public Color GetDataPropertyColor() => string.IsNullOrEmpty(SourceProperty) ? new Color(1f, 0.5f, 0.5f) : Color.white;

    [ChildGameObjectsOnly] 
    [GUIColor(nameof(GetTargetColor))]
    public Component Target;

    public Color GetTargetColor() => Target == null ? new Color(1f, 0.5f, 0.5f) : Color.white;

    [ValueDropdown(nameof(GetTargetPropertyList))] 
    [GUIColor(nameof(GetTargetPropertyColor))]
    [LabelText(" ")]
    [ShowIf(nameof(Target), null)]
    public string TargetProperty;

    public Color GetTargetPropertyColor() => string.IsNullOrEmpty(TargetProperty) ? new Color(1f, 0.5f, 0.5f) : Color.white;

    public ValueDropdownList<string> GetTargetPropertyList()
    {
        var list = new ValueDropdownList<string> { { "NULL", "" } };
        RefreshCache();
        if (TargetType == null) return list;
        Type filterType = null;
        if (SourcePropertyInfo != null) filterType = SourcePropertyInfo.PropertyType;
        if (SourceFieldInfo != null) filterType = SourceFieldInfo.FieldType;
        if (filterType == null) return list;
        var properties = TargetType.GetProperties();
        foreach (var propertyInfo in properties)
        {
            if (propertyInfo.PropertyType != filterType) continue;
            var item = new ValueDropdownItem<string>("Property / " + propertyInfo.PropertyType.Name + " / " + propertyInfo.Name, propertyInfo.Name);
            list.Add(item);
        }

        var fields = TargetType.GetFields();
        foreach (var fieldInfo in fields)
        {
            if (fieldInfo.FieldType != filterType) continue;
            var item = new ValueDropdownItem<string>("Filed / " + fieldInfo.FieldType.Name + " / " + fieldInfo.Name, fieldInfo.Name);
            list.Add(item);
        }

        return list;
    }

    [NonSerialized] public GameObject Owner;
    [NonSerialized] public object SourceData;
    [NonSerialized] public Type SourceType;
    [NonSerialized] public Type TargetType;
    [NonSerialized] public Type TargetPropertyType;
    [NonSerialized] public PropertyInfo SourcePropertyInfo;
    [NonSerialized] public FieldInfo SourceFieldInfo;
    [NonSerialized] public PropertyInfo TargetPropertyInfo;
    [NonSerialized] public FieldInfo TargetFieldInfo;

    public virtual void RefreshCache()
    {
        if (TargetType == null && Target != null)
        {
            TargetType = Target.GetType();
        }

        if (SourcePropertyInfo == null && SourceFieldInfo == null && SourceType != null)
        {
            if (!string.IsNullOrEmpty(SourceProperty))
            {
                SourcePropertyInfo = SourceType.GetProperty(SourceProperty);
                SourceFieldInfo = SourceType.GetField(SourceProperty);
            }
        }

        if (TargetPropertyInfo == null && TargetFieldInfo == null && TargetType != null)
        {
            if (!string.IsNullOrEmpty(TargetProperty))
            {
                TargetPropertyInfo = TargetType.GetProperty(TargetProperty);
                if (TargetPropertyInfo != null) TargetPropertyType = TargetPropertyInfo.PropertyType;
                TargetFieldInfo = TargetType.GetField(TargetProperty);
                if (TargetFieldInfo != null) TargetPropertyType = TargetFieldInfo.FieldType;
            }
        }
    }

    public virtual object GetSourceValue()
    {
        if (SourceData == null) return null;
        if (SourceFieldInfo != null) return SourceFieldInfo.GetValue(SourceData);
        if (SourcePropertyInfo != null) return SourcePropertyInfo.GetValue(SourceData);
        return default;
    }

    public virtual void SetTargetValue(object value)
    {
        if (Target == null) return;
        var setValue = UIDataBinderMap.DefaultConverter.To(value, TargetPropertyType);
        if (TargetFieldInfo != null) TargetFieldInfo.SetValue(Target, setValue);
        if (TargetPropertyInfo != null) TargetPropertyInfo.SetValue(Target, setValue);
    }
}