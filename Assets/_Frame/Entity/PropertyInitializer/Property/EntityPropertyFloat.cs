using System;

[Serializable]
public class EntityPropertyFloat : EntityProperty<float>
{
    public float Value;

    public override object GetValue()
    {
        return Value;
    }
}
