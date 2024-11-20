using System;
using System.Collections.Generic;
using System.Reflection;
using Aya.Extension;
using UnityEngine;

public abstract partial class EntityBase
{
    #region Sub Pool Instance

    public virtual bool SupportSubPoolInstance => false;

    public static Dictionary<Type, List<PropertyInfo>> SubPoolInstancePropertyDic { get; set; } = new Dictionary<Type, List<PropertyInfo>>();
    public static Dictionary<Type, List<FieldInfo>> SubPoolInstanceFieldDic { get; set; } = new Dictionary<Type, List<FieldInfo>>();

    public virtual void CacheSubPoolInstance()
    {
        if (!SupportSubPoolInstance) return;

        var type = GetType();
        if (!SubPoolInstancePropertyDic.TryGetValue(type, out var propertyInfoList))
        {
            propertyInfoList = this.GetPropertiesWithAttribute<SubPoolInstanceAttribute>();
            SubPoolInstancePropertyDic.Add(type, propertyInfoList);
        }

        if (!SubPoolInstanceFieldDic.TryGetValue(type, out var filedInfoList))
        {
            filedInfoList = this.GetFieldsWithAttribute<SubPoolInstanceAttribute>();
            SubPoolInstanceFieldDic.Add(type, filedInfoList);
        }
    }

    public virtual void DeSpawnSubPoolInstance()
    {
        if (!SupportSubPoolInstance) return;

        var type = GetType();
        var propertyInfoList = SubPoolInstancePropertyDic[type];
        foreach (var propertyInfo in propertyInfoList)
        {
            var instance = propertyInfo.GetValue(this);
            if (instance == null) continue;
            if (instance is GameObject go)
            {
                GamePool?.DeSpawn(go);
            }
            else if (instance is MonoBehaviour behaviour)
            {
                GamePool?.DeSpawn(behaviour.gameObject);
            }

            propertyInfo.SetValue(this, null);
        }

        var filedInfoList = SubPoolInstanceFieldDic[type];
        foreach (var fieldInfo in filedInfoList)
        {
            var instance = fieldInfo.GetValue(this);
            if (instance == null) continue;
            if (instance is GameObject go)
            {
                GamePool?.DeSpawn(go);
            }
            else if (instance is MonoBehaviour behaviour)
            {
                GamePool?.DeSpawn(behaviour.gameObject);
            }

            fieldInfo.SetValue(this, null);
        }
    }

    #endregion

}
