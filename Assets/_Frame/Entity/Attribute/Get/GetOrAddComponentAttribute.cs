using System;

[Serializable]
public class GetOrAddComponentAttribute : SetValueEntityPropertyAttribute
{
    public override void SetValue(EntityBase entity, bool isCollectMember, Type valueType, Action<object> setValueAction)
    {
        var component = entity.GetComponent(valueType);
        if (component == null) component = entity.gameObject.AddComponent(valueType);
        setValueAction(component);
    }
}
