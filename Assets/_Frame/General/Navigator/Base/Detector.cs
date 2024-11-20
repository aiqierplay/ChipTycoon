using System;
using UnityEngine;

[Serializable]
public abstract class Detector
{
    public float Weight = 1f;

    public virtual float GetValue(EntityBase handler, Vector3 direction)
    {
        var value = GetValueImpl(handler, direction);
        value *= Weight;
        return value;
    }

    public abstract float GetValueImpl(EntityBase handler, Vector3 direction);
}
