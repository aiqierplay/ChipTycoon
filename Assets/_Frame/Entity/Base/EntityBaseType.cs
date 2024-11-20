using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Aya.Extension;
using Sirenix.OdinInspector;

public abstract partial class EntityBase
{
    #region Property & Field

    public static Dictionary<Type, List<PropertyInfo>> PropertyDic { get; set; } = new Dictionary<Type, List<PropertyInfo>>();
    public static Dictionary<Type, List<FieldInfo>> FieldDic { get; set; } = new Dictionary<Type, List<FieldInfo>>();

    public virtual void CacheProperty()
    {
        var type = GetType();
        GetPropertyInfoList(type);
    }

    public virtual List<PropertyInfo> GetPropertyInfoList(Type type)
    {
        if (PropertyDic.TryGetValue(type, out var propertyInfoList)) return propertyInfoList;
        propertyInfoList = type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic).ToList();
        PropertyDic.Add(type, propertyInfoList);
        return propertyInfoList;
    }

    public virtual void CacheFiled()
    {
        var type = GetType();
        GetFieldInfoList(type);
    }

    public virtual List<FieldInfo> GetFieldInfoList(Type type)
    {
        if (FieldDic.TryGetValue(type, out var fieldInfoList)) return fieldInfoList;
        fieldInfoList = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic).ToList();
        FieldDic.Add(type, fieldInfoList);
        return fieldInfoList;
    }

    public void AutoCacheSubClassList<T>(List<T> list)
    {
        var itemType = typeof(T);
        GetFieldInfoList(GetType()).ForEach(f =>
        {
            if (f.GetCustomAttribute<NonSerializedAttribute>() != null) return;
            if (f.FieldType.IsGenericList() && f.FieldType.GenericTypeArguments[0].IsSubclassOf(itemType))
            {
                if (f.GetValue(this) is IEnumerable<T> effectList)
                {
                    list.AddRange(effectList);
                }
            }
            else if (f.FieldType.IsSubclassOf(itemType))
            {
                if (f.GetValue(this) is T effect)
                {
                    list.Add(effect);
                }
            }
        });
    }

    #endregion

    #region Get DropdownList (Odin Editor Only)

    public ValueDropdownList<string> GetFiledDropdownList(Type[] filterTypes = null) => GetFiledDropdownListByType(GetType(), filterTypes);

    public ValueDropdownList<string> GetFiledDropdownListByType(Type type, Type[] filterTypes = null)
    {
        var dropdown = new ValueDropdownList<string> { { "<None>", "" } };
        if (type == null) return dropdown;
        foreach (var fieldInfo in type.GetFields())
        {
            if (filterTypes != null && !filterTypes.Contains(fieldInfo.FieldType)) continue;
            var key = fieldInfo.Name;
            dropdown.Add("Field / " + key, key);
        }

        return dropdown;
    }

    public ValueDropdownList<string> GetPropertyDropdownList(Type[] filterTypes = null) => GetPropertyDropdownListByType(GetType(), filterTypes);

    public ValueDropdownList<string> GetPropertyDropdownListByType(Type type, Type[] filterTypes = null)
    {
        var dropdown = new ValueDropdownList<string> { { "<None>", "" } };
        if (type == null) return dropdown;
        foreach (var propertyInfo in type.GetProperties())
        {
            if (filterTypes != null && !filterTypes.Contains(propertyInfo.PropertyType)) continue;
            var key = propertyInfo.Name;
            dropdown.Add("Property / " + key, key);
        }

        return dropdown;
    }

    public ValueDropdownList<string> GetFiledAndPropertyDropdownList(Type[] filterTypes = null) => GetFiledAndPropertyDropdownListByType(GetType(), filterTypes);

    public ValueDropdownList<string> GetFiledAndPropertyDropdownListByType(Type type, Type[] filterTypes = null)
    {
        var dropdown = new ValueDropdownList<string>();
        if (type == null) return dropdown;
        dropdown.AddRange(GetFiledDropdownListByType(type, filterTypes));
        dropdown.AddRange(GetPropertyDropdownListByType(type, filterTypes));
        return dropdown;
    }

    #endregion
}