using System;
using Aya.Util;

[Serializable]
public class EntityPropertyIntRange : EntityProperty<int>
{
    public int From;
    public int To;

    public override object GetValue()
    {
        return RandUtil.RandInt(From, To);
    }
}