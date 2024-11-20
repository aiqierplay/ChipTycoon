using System;

[Serializable]
public class GetComponentInChildrenAttribute : SetValueEntityPropertyAttribute
{
    public bool IncludeInactive = false;

    public GetComponentInChildrenAttribute()
    {
    }

    public GetComponentInChildrenAttribute(bool includeInactive = false)
    {
        IncludeInactive = includeInactive;
    }

    public override void SetValue(EntityBase entity, bool isCollectMember, Type valueType, Action<object> setValueAction)
    {
        if (isCollectMember)
        {
            var components = entity.GetComponentsInChildren(valueType, IncludeInactive);
            setValueAction(components);
        }
        else
        {
            var component = entity.GetComponentInChildren(valueType, IncludeInactive);
            setValueAction(component);
        }
    }
}
