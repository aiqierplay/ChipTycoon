using System;

[Serializable]
public class EntityPropertyInt : EntityProperty<int>
{
    public int Value;

    public override object GetValue()
    {
        return Value;
    }
}
