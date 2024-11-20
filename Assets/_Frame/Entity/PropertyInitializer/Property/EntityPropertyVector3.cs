using System;
using UnityEngine;

[Serializable]
public class EntityPropertyVector3 : EntityProperty<Vector3>
{
    public Vector3 Value;

    public override object GetValue()
    {
        return Value;
    }
}
