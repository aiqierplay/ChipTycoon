using System;
using System.Collections.Generic;
using System.Reflection;
using Aya.Data;
using Aya.Extension;

public abstract partial class EntityBase
{
    #region Entity Property Attribute

    public virtual bool EnableAwakeProcessEntityProperty => true;
    public virtual bool EnableEnableProcessEntityProperty => true;
    public virtual bool EnableDisableProcessEntityProperty => true;
    public virtual bool EnableDestroyProcessEntityProperty => true;

    public static MultiDictionary<Type, EntityPropertyMode, Dictionary<PropertyInfo, List<EntityPropertyAttribute>>> EntityPropertyAttributeDic { get; set; } = new MultiDictionary<Type, EntityPropertyMode, Dictionary<PropertyInfo, List<EntityPropertyAttribute>>>();
    public static MultiDictionary<Type, EntityPropertyMode, Dictionary<FieldInfo, List<EntityPropertyAttribute>>> EntityFieldAttributeDic { get; set; } = new MultiDictionary<Type, EntityPropertyMode, Dictionary<FieldInfo, List<EntityPropertyAttribute>>>();

    public static Dictionary<PropertyInfo, List<EntityPropertyAttribute>> GetPropertyAttributeList(Type type, EntityPropertyMode mode)
    {
        if (EntityPropertyAttributeDic.TryGetValue(type, mode, out var propertyAttributeList)) return propertyAttributeList;
        propertyAttributeList = type.GetPropertiesWithAttribute<EntityPropertyAttribute>(p => p.Mode == mode);
        if (propertyAttributeList.Count == 0) propertyAttributeList = null;
        EntityPropertyAttributeDic.Add(type, mode, propertyAttributeList);
        return propertyAttributeList;
    }

    public static Dictionary<FieldInfo, List<EntityPropertyAttribute>> GetFieldAttributeList(Type type, EntityPropertyMode mode)
    {
        if (EntityFieldAttributeDic.TryGetValue(type, mode, out var propertyAttributeList)) return propertyAttributeList;
        propertyAttributeList = type.GetFieldsWithAttribute<EntityPropertyAttribute>(p => p.Mode == mode);
        if (propertyAttributeList.Count == 0) propertyAttributeList = null;
        EntityFieldAttributeDic.Add(type, mode, propertyAttributeList);
        return propertyAttributeList;
    }

    public virtual void ProcessEntityPropertyAttribute(EntityPropertyMode mode)
    {
        var type = GetType();
        var propertyAttributeList = GetPropertyAttributeList(type, mode);
        if (propertyAttributeList != null)
        {
            foreach (var kv in propertyAttributeList)
            {
                var propertyInfo = kv.Key;
                var attributes = kv.Value;
                var count = attributes.Count;
                for (var i = 0; i < count; i++)
                {
                    var attribute = attributes[i];
                    attribute.Process(this, propertyInfo);
                }
            }
        }

        var fieldAttributeList = GetFieldAttributeList(type, mode);
        if (fieldAttributeList != null)
        {
            foreach (var kv in fieldAttributeList)
            {
                var fieldInfo = kv.Key;
                var attributes = kv.Value;
                var count = attributes.Count;
                for (var i = 0; i < count; i++)
                {
                    var attribute = attributes[i];
                    attribute.Process(this, fieldInfo);
                }
            }
        }
    }

    #endregion
}