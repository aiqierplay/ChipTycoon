using System;
using Aya.Util;

[Serializable]
public class EntityPropertyFloatRange : EntityProperty<float>
{
    public float From;
    public float To;

    public override object GetValue()
    {
        return RandUtil.RandFloat(From, To);
    }
}