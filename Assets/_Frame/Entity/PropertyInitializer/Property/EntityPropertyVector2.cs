using System;
using UnityEngine;

[Serializable]
public class EntityPropertyVector2 : EntityProperty<Vector2>
{
    public Vector2 Value;

    public override object GetValue()
    {
        return Value;
    }
}
