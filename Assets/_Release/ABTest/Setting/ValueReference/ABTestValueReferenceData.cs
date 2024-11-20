using Sirenix.OdinInspector;
using System;
using UnityEngine;

[Serializable]
public class ABTestValueReferenceData<T>
{
    [GUIColor(1f, 1f, 0.5f)]
    public string ConfigKey;
    [SerializeReference] public T RefValue;

    [NonSerialized] public bool IsCurrentConfig;
}
