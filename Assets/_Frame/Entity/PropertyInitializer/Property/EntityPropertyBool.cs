using System;

[Serializable]
public class EntityPropertyBool : EntityProperty<bool>
{
    public bool Value;

    public override object GetValue()
    {
        return Value;
    }
}
