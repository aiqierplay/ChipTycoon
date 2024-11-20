using System;
using System.Collections.Generic;
using Aya.Extension;
using Sirenix.OdinInspector;

[Serializable]
public class BattleEquipment
{
    [TableList] public List<BattleEquipmentMountPoint> MountPointList;
    public List<BattleEquipmentBase> EquipmentList = new List<BattleEquipmentBase>();

    [NonSerialized] public BattleEntity Battle;
    public Dictionary<BattleEquipmentType, BattleEquipmentMountPoint> MountPointDic;

    public void Init(BattleEntity battle)
    {
        EquipmentList.Clear();
        MountPointDic = MountPointList.ToDictionary(m => m.Type);
        Battle = battle;
    }

    public void AddEquipment(BattleEquipmentBase equipment)
    {
        EquipmentList.Add(equipment);
        RefreshProperty();
    }

    public void RemoveEquipment(BattleEquipmentBase equipment)
    {
        if (!EquipmentList.Contains(equipment)) return;
        EquipmentList.Remove(equipment);
        RefreshProperty();
    }

    public void RefreshProperty()
    {
        Battle.Property.Copy(Battle.BaseProperty);
        foreach (var equipment in EquipmentList)
        {
            Battle.Property.AddProperty(equipment.Property);
        }
    }
}
