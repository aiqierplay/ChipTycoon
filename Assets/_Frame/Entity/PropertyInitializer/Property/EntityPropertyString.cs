using System;

[Serializable]
public class EntityPropertyString : EntityProperty<string>
{
    public string Value;

    public override object GetValue()
    {
        return Value;
    }
}
