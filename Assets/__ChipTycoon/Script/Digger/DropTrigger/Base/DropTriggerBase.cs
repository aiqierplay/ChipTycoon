using Aya.Physical;
using System;
using System.Collections.Generic;

public abstract class DropTriggerBase : EntityBase
{
    [GetComponentInChildren, NonSerialized] public ColliderListenerEnter ColliderListenerEnter;
    [NonSerialized] public HashSet<DropBase> DropList = new HashSet<DropBase>();

    public virtual void Init()
    {
        gameObject.SetActive(true);
        ColliderListenerEnter.Clear();
        ColliderListenerEnter.onTriggerEnter.Add<DropBase>(OnEnter, LayerManager.Ins.DropItem);
        DropList.Clear();
    }

    public virtual void OnEnter(DropBase dropItem)
    {
        if (DropList.Contains(dropItem)) return;
        DropList.Add(dropItem);
        OnEnterImpl(dropItem);
    }

    public abstract void OnEnterImpl(DropBase dropItem);
}
