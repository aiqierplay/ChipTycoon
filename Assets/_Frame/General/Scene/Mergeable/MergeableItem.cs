using System;
using UnityEngine;
using Sirenix.OdinInspector;

public abstract class MergeableItem<TItem, TData> : DraggableItem
    where TData : MergeableData<TItem>
    where TItem : MergeableItem<TItem, TData>
{
    [Title("Mergeable")]
    public GameObject MergeFx;

    [NonSerialized] public TData Data;

    public virtual void Init(TData data, DroppableArea initArea = null)
    {
        Data = data;
        Init(initArea);
    }
}
