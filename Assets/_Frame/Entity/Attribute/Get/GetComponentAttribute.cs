using System;

[Serializable]
public class GetComponentAttribute : SetValueEntityPropertyAttribute
{
    public override void SetValue(EntityBase entity, bool isCollectMember, Type valueType, Action<object> setValueAction)
    {
        if (isCollectMember)
        {
            var components = entity.GetComponents(valueType);
            setValueAction(components);
        }
        else
        {
            var component = entity.GetComponent(valueType);
            setValueAction(component);
        }
    }
}
