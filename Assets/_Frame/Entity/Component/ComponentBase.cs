using System;
using System.Collections.Generic;
using Aya.Extension;

public abstract class ComponentBase<T> : EntityBase where T : ComponentBase<T>
{
    public Dictionary<Type, T> ComponentDic { get; set; } = new Dictionary<Type, T>();

    public override bool SupportSubPoolInstance => true;

    public virtual void InitAllComponent()
    {
        foreach (var component in ComponentDic.Values)
        {
            component.InitComponent();
        }

        InitAnimatorParameters();
    }

    public virtual void InitComponent()
    {

    }

    public override void CacheComponent()
    {
        base.CacheComponent();

        ComponentDic.Clear();
        var type = typeof(T);
        var fieldInfoList = GetFieldInfoList(type);
        foreach (var fieldInfo in fieldInfoList)
        {
            if (fieldInfo.FieldType.IsSubclassOf(type))
            {
                var component = fieldInfo.GetValue(this) as T;
                if (component == null) continue;
                ComponentDic.TryAdd(fieldInfo.FieldType, component);
            }
        }

        var propertyInfoList = GetPropertyInfoList(type);
        foreach (var propertyInfo in propertyInfoList)
        {
            if (propertyInfo.PropertyType.IsSubclassOf(type))
            {
                var component = propertyInfo.GetValue(this) as T;
                if (component == null) continue;
                ComponentDic.TryAdd(propertyInfo.PropertyType, component);
            }
        }
    }

    public TComponent Get<TComponent>() where TComponent : T
    {
        var type = typeof(TComponent);
        if (ComponentDic.TryGetValue(type, out var component))
        {
            return component as TComponent;
        }

        return default;
    }
}