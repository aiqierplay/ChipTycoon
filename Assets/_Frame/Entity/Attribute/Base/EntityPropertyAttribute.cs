using System;
using System.Reflection;

public enum EntityPropertyMode
{
    Awake = 0,
    Enable = 1,
    Disable = 2,
    Destroy = 3,

    Init = 100,
}

[Serializable]
public abstract class EntityPropertyAttribute : Attribute
{
    public EntityPropertyMode Mode = EntityPropertyMode.Awake;

    public abstract void Process(EntityBase entity, PropertyInfo propertyInfo);

    public abstract void Process(EntityBase entity, FieldInfo fieldInfo);
}