using System;
using System.Collections;
using System.Reflection;
using Aya.Extension;
using Sirenix.OdinInspector;

public enum ItemChangePropertyValueTargetMode
{
    SpecifyTarget = 0,
    TriggerTarget = 1,
}

public class ItemChangePropertyValue : ItemChangeValue<EntityBase>
{
    [Title("Target Property")]
    [FoldoutGroup("Change Value")] public ItemChangePropertyValueTargetMode Mode;
    [FoldoutGroup("Change Value"), ShowIf(nameof(Mode), ItemChangePropertyValueTargetMode.SpecifyTarget)] public EntityBase ChangeTarget;
    [FoldoutGroup("Change Value"), ValueDropdown(nameof(GetItemInitableProperties), DropdownTitle = "Select Property")] public string Property;
    [FoldoutGroup("Change Value")] public bool DeSpawnTrigger = true;

    public IEnumerable GetItemInitableProperties()
    {
        if (ChangeTarget == null) return null;
        return GetFiledAndPropertyDropdownListByType(ChangeTarget.GetType(), new[] { typeof(int), typeof(float) });
    }

    [NonSerialized] public FieldInfo FieldInfo;
    [NonSerialized] public PropertyInfo PropertyInfo;

    public override void Init()
    {
        base.Init();
        FieldInfo = ChangeTarget.GetType().GetField(Property, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
        PropertyInfo = ChangeTarget.GetType().GetProperty(Property, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
    }

    public override float GetValue(EntityBase target)
    {
        var getTarget = Mode == ItemChangePropertyValueTargetMode.SpecifyTarget ? ChangeTarget : target;
        if (FieldInfo != null) return FieldInfo.GetValue(getTarget).CastType<float>();
        if (PropertyInfo != null) return PropertyInfo.GetValue(getTarget).CastType<float>();
        return default;
    }

    public override void SetValue(EntityBase target, float value)
    {
        var setTarget = Mode == ItemChangePropertyValueTargetMode.SpecifyTarget ? ChangeTarget : target;
        object setValue;
        if (FieldInfo != null)
        {
            if (FieldInfo.FieldType == typeof(int)) setValue = value.CastType<int>();
            else setValue = value.CastType<float>();
            FieldInfo.SetValue(setTarget, setValue);
        }

        if (PropertyInfo != null)
        {
            if (PropertyInfo.PropertyType == typeof(int)) setValue = value.CastType<int>();
            else setValue = value.CastType<float>();
            PropertyInfo.SetValue(setTarget, setValue);
        }

        if (DeSpawnTrigger)
        {
            GamePool.DeSpawn(target);
        }

        if (setTarget is ItemBase item)
        {
            this.ExecuteEndOfFrame(item.Refresh);
        }
    }
}
