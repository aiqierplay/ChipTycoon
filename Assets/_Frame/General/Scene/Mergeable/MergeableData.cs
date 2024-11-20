using System;

[Serializable]
public abstract class MergeableData<TItem> : CostConfigData
{
    public TItem Prefab;
}
