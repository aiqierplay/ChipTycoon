using System;
using UnityEngine;

[Serializable]
public class FindAttribute : SetValueEntityPropertyAttribute
{
    public string Name;


    public FindAttribute(string name)
    {
        Name = name;
    }

    public override void SetValue(EntityBase entity, bool isCollectMember, Type valueType, Action<object> setValueAction)
    {
        var transform = entity.Trans.Find(Name);
        if (transform == null) return;
        if (valueType == typeof(GameObject))
        {
            setValueAction(transform.gameObject);
        }
        else
        {
            var component = transform.GetComponent(valueType);
            setValueAction(component);
        }
    }
}
