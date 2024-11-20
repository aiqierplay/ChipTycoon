using System;
using Aya.Extension;

public enum BattleEquipmentType
{
    Default = 0,
}

[Serializable]
public abstract class BattleEquipmentBase : EntityBase
{
    public abstract BattleEquipmentType Type { get; }
    public float Scale = 1f;
    public BattleProperty Property;

    [NonSerialized] public BattleEntity Battle;

    public virtual void Init()
    {
        Property.Reset();
        Battle = null;
    }

    public virtual void Equip(BattleEntity battle)
    {
        Battle = battle;
        var mountPoint = Battle.Equipment.MountPointDic[Type];
        Parent = mountPoint.Point;
        Trans.ResetLocal();
        LocalScaleValue = Scale;
    }
}
