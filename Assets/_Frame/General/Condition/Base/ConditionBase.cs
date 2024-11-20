using System;

[Serializable]
public abstract class ConditionBase
{
}

[Serializable]
public abstract class ConditionBase<TTarget> : ConditionBase
{
    public abstract bool CheckCondition(TTarget target);
}