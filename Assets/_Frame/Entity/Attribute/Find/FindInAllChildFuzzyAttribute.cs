using System;
using Aya.Extension;
using UnityEngine;

[Serializable]
public class FindInAllChildFuzzyAttribute : SetValueEntityPropertyAttribute
{
    public string Name;

    public FindInAllChildFuzzyAttribute(string name)
    {
        Name = name;
    }

    public override void SetValue(EntityBase entity, bool isCollectMember, Type valueType, Action<object> setValueAction)
    {
        var transform = entity.Trans.FindInAllChildFuzzy(Name);
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
