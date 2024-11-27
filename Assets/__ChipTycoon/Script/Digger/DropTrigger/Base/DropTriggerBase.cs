using Aya.Physical;
using System;
using System.Collections.Generic;

public abstract class DropTriggerBase : EntityBase
{
    [GetComponentInChildren, NonSerialized] public ColliderListenerEnter ColliderListenerEnter;
    [NonSerialized] public HashSet<DropBase> DropList = new HashSet<DropBase>();

    public UTweenPlayerReference TweenTrigger;

    public virtual void Init()
    {
        gameObject.SetActive(true);
        ColliderListenerEnter.Clear();
        ColliderListenerEnter.onTriggerEnter.Add<DropBase>(OnEnter, LayerManager.Ins.DropItem);
        DropList.Clear();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        DropList.Clear();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        DropList.Clear();
        StopAllCoroutines();
    }

    public virtual void OnEnter(DropBase dropItem)
    {
        if (DropList.Contains(dropItem)) return;
        DropList.Add(dropItem);
        OnEnterImpl(dropItem);
        TweenTrigger.Play();
    }

    public abstract void OnEnterImpl(DropBase dropItem);
}
