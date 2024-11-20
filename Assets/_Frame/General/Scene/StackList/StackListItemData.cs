using System;
using UnityEngine;

[Serializable]
public class StackListItemData
{
    public Vector3 Position;
    public Vector3 EulerAngle;
    public Vector3 Scale;

    public void Set(EntityBase entity)
    {
        entity.LocalPosition = Position;
        entity.LocalEulerAngles = EulerAngle;
        entity.LocalScale = Scale;
    }
}