using System;

[Serializable]
public class GetComponentInAllAttribute : SetValueEntityPropertyAttribute
{
    public bool IncludeInactive = false;

    public GetComponentInAllAttribute()
    {
    }

    public GetComponentInAllAttribute(bool includeInactive = false) 
    {
        IncludeInactive = includeInactive;
    }

    public override void SetValue(EntityBase entity, bool isCollectMember, Type valueType, Action<object> setValueAction)
    {
        if (isCollectMember)
        {
            var components = entity.GetComponents(valueType);
            if (components == null) entity.GetComponentsInChildren(valueType, IncludeInactive);
            if (components == null) entity.GetComponentsInParent(valueType);
            setValueAction(components);
        }
        else
        {
            var component = entity.GetComponent(valueType);
            if (component == null) entity.GetComponentInChildren(valueType, IncludeInactive);
            if (component == null) entity.GetComponentInParent(valueType);
            setValueAction(component);
        }
    }
}