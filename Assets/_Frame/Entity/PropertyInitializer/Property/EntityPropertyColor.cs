using System;
using UnityEngine;

[Serializable]
public class EntityPropertyColor : EntityProperty<Color>
{
    public Color Value = Color.white;

    public override object GetValue()
    {
        return Value;
    }
}
