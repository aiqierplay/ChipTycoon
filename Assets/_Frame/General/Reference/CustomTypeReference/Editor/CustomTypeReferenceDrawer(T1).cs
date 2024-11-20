#if UNITY_EDITOR
using System.Collections.Generic;
using Aya.Reflection;
using UnityEditor;
using UnityEngine;

public abstract class CustomTypeReferenceDrawer<T> : SearchableDropdownDrawer<T> where T : MonoBehaviour
{
    public SerializedProperty ValueProperty;

    public override void CacheProperty(SerializedProperty property)
    {
        ValueProperty = property.FindPropertyRelative("Value");
    }

    public override T GetValue()
    {
        return ValueProperty.objectReferenceValue as T;
    }

    public override void SetValue(T value)
    {
        ValueProperty.objectReferenceValue = value;
    }

    public override string GetDisplayName(T value)
    {
        var name = value.gameObject.name;
        return name;
    }

    public override string GetRootName()
    {
        return typeof(T).Name;
    }

    public override IEnumerable<T> GetValueCollection()
    {
        var monoBehaviour = Property.serializedObject.targetObject as MonoBehaviour;
        if (monoBehaviour == null) return default;
        var go = monoBehaviour.gameObject;
        var componentList = go.GetComponentsInChildren<T>(true);
        return componentList;
    }
}

#endif