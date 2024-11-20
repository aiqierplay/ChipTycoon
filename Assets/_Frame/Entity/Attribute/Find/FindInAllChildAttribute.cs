using System;
using Aya.Extension;
using UnityEngine;

[Serializable]
public class FindInAllChildAttribute : SetValueEntityPropertyAttribute
{
    public string Name;
    public bool IncludeInactive;

    public FindInAllChildAttribute(string name, bool includeInactive = false)
    {
        Name = name;
        IncludeInactive = includeInactive;
    }

    public override void SetValue(EntityBase entity, bool isCollectMember, Type valueType, Action<object> setValueAction)
    {
        var transform = entity.Trans.FindInAllChild(Name, IncludeInactive);
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
