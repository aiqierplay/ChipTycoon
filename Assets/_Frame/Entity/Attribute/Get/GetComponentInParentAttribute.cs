using System;

[Serializable]
public class GetComponentInParentAttribute : SetValueEntityPropertyAttribute
{
    public bool IncludeInactive = false;

    public GetComponentInParentAttribute()
    {
    }

    public GetComponentInParentAttribute(bool includeInactive = false)
    {
        IncludeInactive = includeInactive;
    }

    public override void SetValue(EntityBase entity, bool isCollectMember, Type valueType, Action<object> setValueAction)
    {
        if (isCollectMember)
        {
            var components = entity.GetComponentsInParent(valueType, IncludeInactive);
            setValueAction(components);
        }
        else
        {
            var component = entity.GetComponentInParent(valueType, IncludeInactive);
            setValueAction(component);
        }
    }
}
