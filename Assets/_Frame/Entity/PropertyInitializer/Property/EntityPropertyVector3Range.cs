using System;
using Aya.Util;
using UnityEngine;

[Serializable]
public class EntityPropertyVector3Range : EntityProperty<Vector3>
{
    public Vector3 From;
    public Vector3 To;

    public override object GetValue()
    {
        return RandUtil.RandVector3(From, To);
    }
}