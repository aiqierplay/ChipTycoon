using System;
using Sirenix.OdinInspector;

[Serializable]
public class ValueCompareCondition : ValueCompareCondition<EntityBase>
{
    public override float GetCompareValue(EntityBase target)
    {
        return 0;
    }
}

[Serializable]
public abstract class ValueCompareCondition<TTarget> : ConditionBase<TTarget>
{
    [HorizontalGroup("Compare"), HideLabel] public ConditionCompareType Compare;
    [HorizontalGroup("Compare"), HideLabel] public float Value;

    public abstract float GetCompareValue(TTarget target);

    public override bool CheckCondition(TTarget target)
    {
        return CheckValue(GetCompareValue(target), Value, Compare);
    }

    public virtual bool CheckValue(float srcValue, float compareValue, ConditionCompareType compareType)
    {
        return ConditionCompareUtil.Compare(srcValue, compareValue, compareType);
    }
}