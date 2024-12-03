using System;
using System.Collections;
using System.Collections.Generic;
using Aya.Physical;
using JetBrains.Annotations;
using UnityEngine;

public abstract class BuildingBase : EntityBase
{
    [GetComponentInChildren, NonSerialized] [CanBeNull]
    public List<ColliderListenerEnter> ColliderListenerEnter;

    [GetComponentInChildren, NonSerialized]
    public List<ColliderListenerExit> ColliderListenerExit;

    [NonSerialized] public List<Worker> WorkerList = new List<Worker>();

    public virtual void Init()
    {
        foreach (var enter in ColliderListenerEnter)
        {
            enter.Clear();
            enter.onTriggerEnter.Add<Worker>(OnEnter, LayerManager.Ins.Player);
        }

        foreach (var exit in ColliderListenerExit)
        {
            exit.Clear();
            exit.onTriggerExit.Add<Worker>(OnExit, LayerManager.Ins.Player);
        }
        
        WorkerList.Clear();
    }

    public virtual void Refresh()
    {

    }

    public virtual void OnEnter(Worker worker)
    {
        if (worker.Type != WorkerType.Player) return;
        worker.OnEnter(this);
        OnEnterImpl(worker);
    }

    public virtual void OnExit(Worker worker)
    {
        if (worker.Type != WorkerType.Player) return;
        worker.OnExit(this);
        OnExitImpl(worker);
    }

    public abstract void OnEnterImpl(Worker worker);
    public abstract void OnExitImpl(Worker worker);
}
